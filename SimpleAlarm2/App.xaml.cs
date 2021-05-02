using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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


        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }


        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            //1. 어셈블리 이름을 얻습니다.
            string name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

            //2. Embedded Resources로부터 리소스를 로드합니다.
            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
            if (resources.Any())
            {
                //거의 대부분의 확률로 항상 1개의 항목만 로드됩니다. 만약 1개이상 로드가되면 이러한 케이스를 별도로 처리해야 합니다.
                var resourceName = resources.First();
                using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return null;
                    var block = new byte[stream.Length];
                    stream.Read(block, 0, block.Length);
                    return Assembly.Load(block);
                }
            }
            return null;
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
    }
}
