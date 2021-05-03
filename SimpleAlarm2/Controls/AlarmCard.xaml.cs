using MaterialDesignThemes.Wpf;
using SimpleAlarm2.Dialog;
using SimpleAlarm2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class AlarmCard : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DependencyProperty IndexProperty
            = DependencyProperty.Register("Index", typeof(int), typeof(AlarmCard),
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender, IndexChanged));

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public AlertContent Alarm { get => App.AlarmController.GetPinnedAlert(Index); }


        public AlarmCard()
        {
            InitializeComponent();
            Loaded += (o, e) => App.AlarmController.OnAlertChanged += AlarmController_OnAlertChanged;
            Unloaded += (o, e) => App.AlarmController.OnAlertChanged -= AlarmController_OnAlertChanged;
        }

        private void AlarmController_OnAlertChanged(object sender, int e)
        {
            if(Index == e && Alarm == null) // when the alarm this control owned has been deleted.
                UpdateUI();
        }

        private void UpdateUI()
        {
            OnPropertyChanged("Alarm");
            if (Alarm == null)
            {
                pnlNullContent.Visibility = Visibility.Visible;
                pnlAlarmContent.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlNullContent.Visibility = Visibility.Collapsed;
                pnlAlarmContent.Visibility = Visibility.Visible;
            }
        }

        private void SetAlert_Click(object sender, RoutedEventArgs e)
        {
            DialogHelper.Show(new SetPinnedAlarmDialog(), OnSetPinnedAlarmDialogClosed);
        }

        private void ClearAlert_Click(object sender, RoutedEventArgs e)
        {
            App.AlarmController.SetPinnedAlert(Index, -1);
            App.Settings.Save();
            UpdateUI();
        }

        private void OnSetPinnedAlarmDialogClosed(object o, DialogClosingEventArgs e)
        {
            if ((bool)e.Parameter)
            {
                SetPinnedAlarmDialog dialog = DialogHelper.GetDialog<SetPinnedAlarmDialog>(o);
                int selected = dialog.cbxAlarmType.SelectedIndex;

                if (selected != -1)
                {
                    if (App.AlarmController.SetPinnedAlert(Index, selected))
                    {
                        App.Settings.Save();
                        UpdateUI();
                    }
                    else
                    {
                        MainViewModel.SnackMessageQueue.Enqueue("선택한 알람은 이미 고정되어있습니다.");
                    }
                }
                else
                {
                    MainViewModel.SnackMessageQueue.Enqueue("고정할 알람을 선택해주세요.");
                }
            }
        }

        public static void IndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AlarmCard)d).UpdateUI();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Ripple_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Alarm != null)
            {
                SpecificTimer alarm = Alarm as SpecificTimer;
                if (alarm != null)
                {
                    if (alarm.IsPaused)
                        alarm.PlayCommand.Execute(null);
                    else
                        alarm.PauseCommand.Execute(null);
                }
            }
        }
    }
}
