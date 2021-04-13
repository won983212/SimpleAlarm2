using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2.ViewModels
{
    class TabChild : ObservableObject
    {
        public readonly TabContainer Parent;

        public TabChild(TabContainer parent)
        {
            Parent = parent;
        }
    }
}
