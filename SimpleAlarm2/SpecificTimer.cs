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
        private DateTime endTime;
        private bool _hasSynchronized = false;

        public SpecificTimer(string label, TimeSpan time)
            : base(label, time, SimpleAlarm2.AlertType.Timer)
        {
            RemainingTime = time;
            PlayCommand = new RelayCommand(Play);
            PauseCommand = new RelayCommand(Pause);
            ResetCommand = new RelayCommand(Reset);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_hasSynchronized)
            {
                (sender as DispatcherTimer).Interval = TimeSpan.FromSeconds(1);
                _hasSynchronized = true;
            }

            RemainingTime = endTime - DateTime.Now;
        }

        private void Play()
        {
            IsPaused = false;

            if (timer != null)
                timer.Stop();

            _hasSynchronized = false;
            endTime = DateTime.Now + RemainingTime;

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(RemainingTime.Milliseconds);
            timer.Start();
        }

        private void Pause()
        {
            IsPaused = true;
            RemainingTime = endTime - DateTime.Now;

            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void Reset()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            IsPaused = true;
            RemainingTime = Time;
        }
    }
}
