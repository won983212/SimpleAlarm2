using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    public abstract class AlertContent
    {
        public string Label { get; protected set; }

        public TimeSpan Time { get; protected set; }

        public bool IsAlertEnabled { get; protected set; }


        protected AlertContent(string label, TimeSpan time)
        {
            Label = label;
            Time = time;
        }


        public abstract TimeSpan GetRemainingTime();

        public string GetRemainingTimeString()
        {
            string format = Properties.Settings.Default.UseZeroFillFormat ? @"hh\:mm\:ss" : @"mm\:ss";
            return GetRemainingTime().ToString(format);
        }
    }
}
