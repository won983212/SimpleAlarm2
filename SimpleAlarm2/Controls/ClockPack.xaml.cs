using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimpleAlarm2.Controls
{
    public partial class ClockPack : UserControl
    {
        private bool _hasSynchronized = false;

        public ClockPack()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 - DateTime.Now.Millisecond);
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateTimerUI();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_hasSynchronized)
            {
                (sender as DispatcherTimer).Interval = TimeSpan.FromSeconds(1);
                _hasSynchronized = true;
            }
            UpdateTimerUI();
        }

        private void UpdateTimerUI()
        {
            DateTime time = DateTime.Now;
            analogClock.Time = time;
            tblAmPm.Text = time.ToString("tt", CultureInfo.InvariantCulture);
            tblDate.Text = time.ToString("yyyy.MM.dd ") + GetDayOfWeek(time.DayOfWeek) + ".";
            tblTime.Text = time.ToString("hh:mm");
        }

        public static string GetDayOfWeek(DayOfWeek now)
        {
            switch (now)
            {
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Tuesday:
                    return "Tue";
                case DayOfWeek.Wednesday:
                    return "Wed";
                case DayOfWeek.Thursday:
                    return "Thu";
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Saturday:
                    return "Sat";
                case DayOfWeek.Sunday:
                    return "Sun";
            }

            return "";
        }
    }
}
