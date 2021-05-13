using MaterialDesignThemes.Wpf;
using SimpleAlarm2.Dialog;
using SimpleAlarm2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleAlarm2
{
    public enum AlertType
    {
        Alarm, Timer
    }

    public abstract class AlertContent : ObservableObject
    {
        private bool _isAlertEnabled = true;
        private bool _isPaused = true;
        private string _label = "";
        private TimeSpan _remaining = new TimeSpan(0);
        private TimeSpan _time = new TimeSpan(0);

        protected AlertContent(string label, TimeSpan time, AlertType type)
        {
            DeleteCommand = new RelayCommand(DeleteElement);
            ModifyCommand = new RelayCommand(() => DialogHelper.Show(new ModifyAlarmDialog(this), OnModifyAlarmDialogClosed));
            Label = label;
            Time = time;
            AlertType = (int) type;
        }

        private void DeleteElement()
        {
            App.AlarmController.RemoveAlert(this);
        }

        private void OnModifyAlarmDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                ModifyAlarmDialog dialog = DialogHelper.GetDialog<ModifyAlarmDialog>(o);
                if (dialog.ValidateInput())
                {
                    DateTime? time = dialog.timePicker.SelectedTime;
                    Label = dialog.tbxName.Text;
                    Time = new TimeSpan(time.Value.Hour, time.Value.Minute, time.Value.Second);
                    ResetCommand.Execute(null);
                    App.Settings.Save();
                }
            }
        }

        public virtual void OnRootTimerTick() { }


        #region Properties

        // class properties
        public string Label
        {
            get => _label;
            set { _label = value; OnPropertyChanged(); }
        }

        public TimeSpan Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); OnPropertyChanged("RemainingTime"); }
        }

        public bool IsAlertEnabled
        {
            get => _isAlertEnabled;
            set { _isAlertEnabled = value; App.Settings.Save(); }
        }

        public bool IsPaused
        {
            get => _isPaused;
            set { _isPaused = value; OnPropertyChanged(); }
        }

        public TimeSpan RemainingTime
        {
            get => _remaining;
            set { _remaining = value; OnPropertyChanged(); }
        }

        public int AlertType { get; private set; }

        public ICommand PlayCommand { get; protected set; } = new RelayCommand(() => { });

        public ICommand PauseCommand { get; protected set; } = new RelayCommand(() => { });

        public ICommand ResetCommand { get; protected set; } = new RelayCommand(() => { });

        public ICommand ModifyCommand { get; protected set; }

        public ICommand DeleteCommand { get; protected set; }

        #endregion
    }
}
