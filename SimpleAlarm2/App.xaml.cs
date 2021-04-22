using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleAlarm2
{
    public enum Skin
    {
        Light, Dark
    }

    public partial class App : Application
    {
        private static Skin _skin = Skin.Light;

        public static Skin Skin
        {
            get => _skin;
            set
            {
                if (_skin != value)
                {
                    _skin = value;
                    Current.Resources.MergedDictionaries[2].Source = new Uri("/Theme/Theme." + _skin.ToString() + ".xaml", UriKind.Relative);
                }
            }
        }
    }
}
