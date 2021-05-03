using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    public class SimpleAlarmProperties : BinaryProperties
    {
        public bool UseSnappingWindow
        {
            get => (bool)GetData("UseSnappingWindow", true);
            set => SetData("UseSnappingWindow", value);
        }

        public bool UseDarkMode
        {
            get => (bool)GetData("UseDarkMode", false);
            set => SetData("UseDarkMode", value);
        }

        public bool ShowHomeAlarm
        {
            get => (bool)GetData("ShowHomeAlarm", true);
            set => SetData("ShowHomeAlarm", value);
        }

        public bool UseTopmost
        {
            get => (bool)GetData("UseTopmost", false);
            set => SetData("UseTopmost", value);
        }

        public bool UseClockAnimation
        {
            get => (bool)GetData("UseClockAnimation", true);
            set => SetData("UseClockAnimation", value);
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(UseSnappingWindow);
            writer.Write(UseDarkMode);
            writer.Write(ShowHomeAlarm);
            writer.Write(UseTopmost);
            writer.Write(UseClockAnimation);
            App.AlarmController.Serialize(writer);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            UseSnappingWindow = reader.ReadBoolean();
            UseDarkMode = reader.ReadBoolean();
            ShowHomeAlarm = reader.ReadBoolean();
            UseTopmost = reader.ReadBoolean();
            UseClockAnimation = reader.ReadBoolean();
            App.AlarmController.Deserialize(reader);
        }
    }
}
