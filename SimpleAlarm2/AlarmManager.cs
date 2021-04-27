using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            // TODO(Debug): 나중에 지워
            AddAlert(new Alarm("Test Alarm", new TimeSpan(10, 10, 10)));

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
        }
    }
}
