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
        public ICommand AddAlarmCommand => new RelayCommand(() => DialogHelper.Show(new AddAlarmDialog(), OnAddAlarmDialogClosed));

        public AlarmTabViewModel(TabContainer parent)
            : base(parent)
        { }

        private void OnAddAlarmDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                AddAlarmDialog dialog = DialogHelper.GetDialog<AddAlarmDialog>(o);
                if (dialog.ValidateInput())
                {
                    string name = dialog.tbxName.Text;
                    DateTime? time = dialog.timePicker.SelectedTime;

                    TimeSpan timeSpan = new TimeSpan(time.Value.Hour, time.Value.Minute, time.Value.Second);
                    if (dialog.cbxAlarmType.SelectedIndex == 0)
                        App.AlarmController.AddAlert(new Alarm(name, timeSpan)); // alarm
                    else
                        App.AlarmController.AddAlert(new SpecificTimer(name, timeSpan)); // timer
                }
            }
        }
    }
}
