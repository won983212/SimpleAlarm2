using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleAlarm2.ViewModels.Tabs
{
    class HomeTabViewModel : TabChild
    {
        private DateTime _now;

        public DateTime Now
        {
            get => _now;
            set
            {
                _now = value;
                OnPropertyChanged();
            }
        }

        public HomeTabViewModel(TabContainer parent)
            : base(parent)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, e) => UpdateTime();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            UpdateTime();
        }

        private void UpdateTime()
        {
            Now = DateTime.Now;
        }
    }
}
