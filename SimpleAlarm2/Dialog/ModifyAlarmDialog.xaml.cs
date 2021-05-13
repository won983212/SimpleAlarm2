using SimpleAlarm2.ViewModels;
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
    public partial class ModifyAlarmDialog : UserControl
    {
        public ModifyAlarmDialog(AlertContent alert)
        {
            InitializeComponent();
            cbxAlarmType.SelectedIndex = alert.AlertType;
            tbxName.Text = alert.Label;
            timePicker.SelectedTime = DateTime.Today + alert.Time;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timePicker == null)
                return;

            bool trigger = cbxAlarmType.SelectedIndex != 0;
            timePicker.WithSeconds = trigger;
            timePicker.Is24Hours = trigger;
        }

        public bool ValidateInput()
        {
            string name = tbxName.Text;
            DateTime? time = timePicker.SelectedTime;

            // check name
            if (name.Length == 0)
            {
                MainViewModel.SnackMessageQueue.Enqueue("이름을 입력해주세요.");
                return false;
            }

            // name contains illegal characters
            if (name.Contains('♪') || name.Contains('♬'))
            {
                MainViewModel.SnackMessageQueue.Enqueue("이름에 사용할 수 없는 문자가 포함되어있습니다.");
                return false;
            }

            // check time
            if (time == null)
            {
                MainViewModel.SnackMessageQueue.Enqueue("시각 또는 시간을 제대로 입력해주세요.");
                return false;
            }

            return true;
        }
    }
}
