using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleAlarm2
{
    class SpecificTimer : AlertContent
    {
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private DateTime endTime;
        private bool _hasSynchronized = false;

        public SpecificTimer(string label, TimeSpan time)
            : base(label, time, SimpleAlarm2.AlertType.Timer)
        {
            remainingTime = time;
            PlayCommand = new RelayCommand<object>(Play);
            PauseCommand = new RelayCommand<object>(Pause);
            ResetCommand = new RelayCommand<object>(Reset);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_hasSynchronized)
            {
                (sender as DispatcherTimer).Interval = TimeSpan.FromSeconds(1);
                _hasSynchronized = true;
            }

            remainingTime = endTime - DateTime.Now;
            OnPropertyChanged("TimeString");
        }

        private void Play(object o)
        {
            IsPaused = false;

            if (timer != null)
                timer.Stop();

            _hasSynchronized = false;
            endTime = DateTime.Now + remainingTime;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(remainingTime.Milliseconds);
            timer.Start();
        }

        private void Pause(object o)
        {
            IsPaused = true;
            remainingTime = endTime - DateTime.Now;
            OnPropertyChanged("TimeString");

            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void Reset(object o)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            IsPaused = true;
            remainingTime = Time;
            OnPropertyChanged("TimeString");
        }

        public override string GetAmPmString()
        {
            return "";
        }

        public override string GetTimeString()
        {
            return remainingTime.ToString(@"hh\:mm\:ss");
        }

        public override TimeSpan GetRemainingTime()
        {
            return remainingTime;
        }

        public override void Update() { }
    }
}
