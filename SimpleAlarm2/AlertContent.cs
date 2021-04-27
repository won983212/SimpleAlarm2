using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    public abstract class AlertContent : ObservableObject
    {
        public string Label { get; protected set; }

        public TimeSpan Time { get; protected set; }

        public bool IsAlertEnabled { get; set; }

        public string TimeString { get => GetTimeString(); }

        public string AmPmString { get => GetAmPmString(); }

        private string _remainingTime = "(계산중..)";
        public string RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
            }
        }


        protected AlertContent(string label, TimeSpan time)
        {
            Label = label;
            Time = time;
            Update();
        }


        public abstract TimeSpan GetRemainingTime();

        public abstract string GetTimeString();

        public abstract string GetAmPmString();

        public void Update()
        {
            RemainingTime = GetRemainingTime().ToString(@"hh\:mm\:ss");
        }
    }
}
