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

namespace SimpleAlarm2.Controls
{
    public partial class ClockPack : UserControl
    {
        public static DependencyProperty TimeProperty
            = DependencyProperty.Register("Time", typeof(DateTime), typeof(ClockPack),
                new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.AffectsRender, TimeChanged));

        public DateTime Time
        {
            get => (DateTime) GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public ClockPack()
        {
            InitializeComponent();
        }

        private static void TimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClockPack inst = (ClockPack)d;
            DateTime time = (DateTime)e.NewValue;

            inst.analogClock.Time = time;
            inst.tblAmPm.Text = time.ToString("tt", CultureInfo.InvariantCulture);
            inst.tblDate.Text = time.ToString("yyyy.MM.dd ") + GetDayOfWeek(time.DayOfWeek) + ".";
            inst.tblTime.Text = time.ToString("hh:mm");
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
