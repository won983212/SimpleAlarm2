using SimpleAlarm2.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2.ViewModels
{
    class MainViewModel : TabContainer
    {
        public MainViewModel()
        {
            CurrentPage = new HomeTabViewModel(this);
        }
    }
}
