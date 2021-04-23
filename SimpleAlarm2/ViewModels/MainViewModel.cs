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

        private int _selectedTabItemIndex = 0;
        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
            set
            {
                _selectedTabItemIndex = value;
                OnTabChanged(_selectedTabItemIndex);
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _tabs = new TabChild[] { new HomeTabViewModel(this), new HomeTabViewModel(this), new HomeTabViewModel(this) }; // TODO Tab이 만들어지면 여기에 추가
            SelectedTabItemIndex = 0;
        }

        private void OnTabChanged(int index)
        {
            if(index == 3)
                App.Current.Shutdown();
            else
                CurrentPage = _tabs[index];
        }
    }
}
