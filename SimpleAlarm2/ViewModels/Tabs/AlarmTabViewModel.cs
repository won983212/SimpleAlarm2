using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleAlarm2.ViewModels.Tabs
{
    class AlarmTabViewModel : TabChild
    {
        public ObservableCollection<AlertContent> Alerts { get; set; }

        public AlarmTabViewModel(TabContainer parent)
            : base(parent)
        {
            Alerts = new ObservableCollection<AlertContent>();
            Alerts.Add(new Alarm("Test alarm", TimeSpan.FromMinutes(45)));
        }
    }
}
