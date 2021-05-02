using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static readonly Properties.Settings Settings = SimpleAlarm2.Properties.Settings.Default;
        private static ResourceDictionary cachedSkinDictionary = null;
        private static Skin skin = Skin.Light;
        public static AlarmManager AlarmController { get; private set; } = new AlarmManager();

        public static Skin Skin
        {
            get => skin;
            set
            {
                if (skin != value)
                {
                    skin = value;
                    if (cachedSkinDictionary == null)
                        cachedSkinDictionary = Current.Resources.MergedDictionaries[2];
                    cachedSkinDictionary.Source = new Uri("/Theme/Theme." + skin.ToString() + ".xaml", UriKind.Relative);
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Settings.PropertyChanged += Default_PropertyChanged;
            Skin = Settings.UseDarkMode ? Skin.Dark : Skin.Light;
            AlarmController.LoadAlarms();
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Settings.Save();
            if (e.PropertyName == "UseDarkMode")
                Skin = Settings.UseDarkMode ? Skin.Dark : Skin.Light;
        }
    }
}
