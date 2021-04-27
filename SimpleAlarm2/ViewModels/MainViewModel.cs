using MaterialDesignThemes.Wpf;
using SimpleAlarm2.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleAlarm2.ViewModels
{
    class MainViewModel : TabContainer
    {
        private TabChild[] _tabs;

        public SnackbarMessageQueue SnackMessages { get; private set; }

        private int _selectedTabItemIndex = 0;
        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
            set
            {
                _selectedTabItemIndex = value;
                if (_selectedTabItemIndex == 3)
                    App.Current.Shutdown();
                else
                    CurrentPage = _tabs[_selectedTabItemIndex];
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _tabs = new TabChild[] { new HomeTabViewModel(this), new AlarmTabViewModel(this), new SettingsTabViewModel(this) }; // Tab이 만들어지면 여기에 추가
            SnackMessages = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
            SelectedTabItemIndex = 1; // TODO(Debug): 나중에 기본탭index 0으로 바꾸자
        }

        public void AddErrorSnackbar(string message)
        {
            SnackMessages.Enqueue(message);
        }
    }
}
