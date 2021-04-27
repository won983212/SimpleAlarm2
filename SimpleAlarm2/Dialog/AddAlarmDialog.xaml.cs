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

namespace SimpleAlarm2.Dialog
{
    public partial class AddAlarmDialog : UserControl
    {
        public AddAlarmDialog()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timePicker == null)
                return;

            bool trigger = cbxAlarmType.SelectedIndex != 0;
            timePicker.WithSeconds = trigger;
            timePicker.Is24Hours = trigger;
        }
    }
}
