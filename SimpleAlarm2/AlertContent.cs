using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleAlarm2
{
    public enum AlertType
    {
        Alarm, Timer
    }

    public abstract class AlertContent : ObservableObject
    {
        // class properties
        public string Label { get; protected set; }
        public TimeSpan Time { get; protected set; }
        public bool IsAlertEnabled { get; set; }

        // For xaml compatibility properties
        public string TimeString { get => GetTimeString(); }
        public string AmPmString { get => GetAmPmString(); }
        public int AlertType { get; private set; }
        public string RemainingTime { get => GetRemainingTime().ToString(@"hh\:mm\:ss"); }
        public ICommand PlayCommand { get; protected set; } = new RelayCommand<object>((o) => { });
        public ICommand PauseCommand { get; protected set; } = new RelayCommand<object>((o) => { });
        public ICommand ResetCommand { get; protected set; } = new RelayCommand<object>((o) => { });

        private bool _isPaused = true;
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                OnPropertyChanged();
            }
        }


        protected AlertContent(string label, TimeSpan time, AlertType type)
        {
            Label = label;
            Time = time;
            AlertType = (int) type;
            Update();
        }


        public abstract TimeSpan GetRemainingTime();

        public abstract string GetTimeString();

        public abstract string GetAmPmString();

        public abstract void Update();
    }
}
