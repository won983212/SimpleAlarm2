using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleAlarm2
{
    public class AlarmManager : ObservableObject
    {
        private bool _hasSynchronized = false;

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
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_hasSynchronized)
            {
                (sender as DispatcherTimer).Interval = TimeSpan.FromSeconds(1);
                _hasSynchronized = true;
            }

            foreach (AlertContent content in Alerts)
                content.Update();
        }

        public void AddAlert(AlertContent content)
        {
            Alerts.Add(content);
            OnPropertyChanged("IsAlertEmpty");
            SaveAlarms();
        }

        public void RemoveAlert(AlertContent content)
        {
            Alerts.Remove(content);
            OnPropertyChanged("IsAlertEmpty");
            SaveAlarms();
        }

        public void LoadAlarms()
        {
            Alerts.Clear();
            string prop = Properties.Settings.Default.Alarms;
            if (prop.Length > 0)
            {
                try
                {
                    foreach (string ent in prop.Split('♪'))
                    {
                        string[] tokens = ent.Split('♬');
                        int type = tokens[0][0] - '0';
                        string label = tokens[1];
                        TimeSpan time = TimeSpan.FromSeconds(int.Parse(tokens[2]));

                        if (type == (int)AlertType.Alarm)
                            Alerts.Add(new Alarm(label, time));
                        else if(type == (int)AlertType.Timer)
                            Alerts.Add(new SpecificTimer(label, time));
                    }
                    OnPropertyChanged("IsAlertEmpty");
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Alerts.Clear();
                    SaveAlarms();
                }
            }
            else
            {
                SaveAlarms();
            }
        }

        public void SaveAlarms()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Alerts.Count; i++)
            {
                AlertContent ent = Alerts[i];
                sb.Append(ent.AlertType);
                sb.Append('♬');
                sb.Append(ent.Label);
                sb.Append('♬');
                sb.Append((int)ent.Time.TotalSeconds);
                sb.Append('♪');
            }
            if(sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            Properties.Settings.Default.Alarms = sb.ToString();
        }
    }
}
