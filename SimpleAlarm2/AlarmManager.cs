using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;

namespace SimpleAlarm2
{
    public class AlarmManager : ObservableObject
    {
        public event EventHandler<int> OnAlertChanged;
        public event EventHandler OnTimeChanged;

        private SoundPlayer alarmSound = null;
        private bool _hasSynchronized = false;
        private int[] _pinnedAlerts = new int[4] { -1, -1, -1, -1 };
        public ObservableCollection<AlertContent> Alerts { get; private set; } = new ObservableCollection<AlertContent>();

        public bool IsAlertEmpty
        {
            get => Alerts.Count == 0;
        }

        public AlarmManager()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 - DateTime.Now.Millisecond);
            timer.Tick += Timer_Tick;
            timer.Start();

            // load alarm sound
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("/Resources/alarm.wav", UriKind.Relative));
            if (sri != null)
            {
                using (Stream s = sri.Stream)
                {
                    alarmSound = new SoundPlayer(s);
                    alarmSound.Load();
                }
            }
        }

        private void PlayAlarm()
        {
            if (alarmSound != null)
                alarmSound.Play();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_hasSynchronized)
            {
                (sender as DispatcherTimer).Interval = TimeSpan.FromSeconds(1);
                _hasSynchronized = true;
            }

            foreach (AlertContent content in Alerts)
            {
                content.OnRootTimerTick();
                if ((int)content.RemainingTime.TotalSeconds <= 0)
                {
                    if (content.IsAlertEnabled)
                    {
                        new Notification((content is SpecificTimer ? "타이머" : "알람") + " 만료됨.", content.Label).Show();
                        PlayAlarm();
                    }
                    content.ResetCommand.Execute(null);
                }
            }

            OnTimeChanged?.Invoke(this, null);
        }

        public bool SetPinnedAlert(int index, int alertPos)
        {
            // check duplicate
            foreach(int ent in _pinnedAlerts)
            {
                if (ent != -1 && ent == alertPos)
                    return false;
            }

            _pinnedAlerts[index] = alertPos;
            OnAlertChanged?.Invoke(this, index);
            return true;
        }

        public AlertContent GetPinnedAlert(int index)
        {
            if (index < 0 || index >= _pinnedAlerts.Length)
                return null;

            int ptr = _pinnedAlerts[index];
            if (ptr < 0 || ptr >= Alerts.Count)
                return null;

            return Alerts[_pinnedAlerts[index]];
        }

        public void AddAlert(AlertContent content)
        {
            Alerts.Add(content);
            OnPropertyChanged("IsAlertEmpty");
            App.Settings.Save();
        }

        public void RemoveAlert(AlertContent content)
        {
            int pos = Alerts.IndexOf(content);
            for (int i = 0; i < _pinnedAlerts.Length; i++)
            {
                int idx = _pinnedAlerts[i];
                if (idx >= 0 && idx < Alerts.Count)
                {
                    if (idx == pos)
                        SetPinnedAlert(i, -1);
                    else if(idx > pos)
                        _pinnedAlerts[i]--;
                }
            }
            Alerts.Remove(content);

            OnPropertyChanged("IsAlertEmpty");
            App.Settings.Save();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Alerts.Count);
            foreach (AlertContent content in Alerts)
            {
                writer.Write(content.AlertType);
                writer.Write(content.Label);
                writer.Write((int) content.Time.TotalSeconds);
                writer.Write(content.IsAlertEnabled);
            }
            for (int i = 0; i < _pinnedAlerts.Length; i++)
            {
                writer.Write(_pinnedAlerts[i]);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for(int i = 0; i < len; i++)
            {
                int type = reader.ReadInt32();
                string label = reader.ReadString();
                TimeSpan time = TimeSpan.FromSeconds(reader.ReadInt32());
                bool isEnabled = reader.ReadBoolean();

                AlertContent alert = null;
                if (type == (int)AlertType.Alarm)
                    alert = new Alarm(label, time);
                else if (type == (int)AlertType.Timer)
                    alert = new SpecificTimer(label, time);

                if (alert != null)
                {
                    alert.IsAlertEnabled = isEnabled;
                    Alerts.Add(alert);
                }
            }
            for (int i = 0; i < _pinnedAlerts.Length; i++)
            {
                _pinnedAlerts[i] = reader.ReadInt32();
            }
            OnPropertyChanged("IsAlertEmpty");
        }
    }
}
