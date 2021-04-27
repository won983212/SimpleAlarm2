using MaterialDesignThemes.Wpf;
using SimpleAlarm2.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleAlarm2.ViewModels.Tabs
{
    class AlarmTabViewModel : TabChild
    {
        public ICommand AddAlarmCommand => new RelayCommand<object>((o) => DialogHost.Show(new AddAlarmDialog(), "RootDialogHost", OnAddAlarmDialogClosed));

        public AlarmTabViewModel(TabContainer parent)
            : base(parent)
        { }

        private void OnAddAlarmDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                AddAlarmDialog dialog = DialogHelper.GetDialog<AddAlarmDialog>(o);
                string name = dialog.tbxName.Text;
                DateTime? time = dialog.timePicker.SelectedTime;

                // check name
                if (name.Length == 0)
                {
                    ((MainViewModel)Parent).AddErrorSnackbar("이름을 입력해주세요.");
                    return;
                }

                // check time
                if(time == null)
                {
                    ((MainViewModel)Parent).AddErrorSnackbar("시각 또는 시간을 제대로 입력해주세요.");
                    return;
                }

                TimeSpan timeSpan = new TimeSpan(time.Value.Hour, time.Value.Minute, time.Value.Second);
                if (dialog.cbxAlarmType.SelectedIndex == 0)
                    App.AlarmController.AddAlert(new Alarm(name, timeSpan)); // alarm
                else
                    App.AlarmController.AddAlert(new SpecificTimer(name, timeSpan)); // timer
            }
        }
    }
}
