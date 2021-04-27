using System;
using System.Collections.Generic;
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
    public partial class AlarmCard : UserControl
    {
        public static DependencyProperty AlarmProperty
            = DependencyProperty.Register("Alarm", typeof(AlertContent), typeof(AlarmCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, AlarmChanged));

        public AlertContent Alarm
        {
            get => (AlertContent)GetValue(AlarmProperty);
            set => SetValue(AlarmProperty, value);
        }


        public AlarmCard()
        {
            InitializeComponent();
        }

        public static void AlarmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AlarmCard obj = (AlarmCard)d;
            AlertContent alert = (AlertContent) e.NewValue;

            if(alert == null)
            {
                obj.borderBackground.Background = Brushes.Transparent;
                obj.borderBackground.BorderThickness = new Thickness(1);
                obj.pnlAlarmContent.Visibility = Visibility.Hidden;
            } 
            else
            {
                obj.borderBackground.Background = (Brush) Application.Current.FindResource("CardDark0Color");
                obj.borderBackground.BorderThickness = new Thickness(0);
                obj.pnlAlarmContent.Visibility = Visibility.Visible;
            }
        }
    }
}
