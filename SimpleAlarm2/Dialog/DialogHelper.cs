using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2.Dialog
{
    public static class DialogHelper
    {
        public static T GetDialog<T>(object o)
        {
            return (T)((DialogHost)o).DialogContent;
        }

        public static Task<object> Show(object content, DialogClosingEventHandler closingHandler)
        {
            return DialogHost.Show(content, "RootDialogHost", closingHandler);
        }
    }
}
