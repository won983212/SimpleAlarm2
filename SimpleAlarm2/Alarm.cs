using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    public class Alarm : AlertContent
    {
        public Alarm(string label, TimeSpan destTime)
            : base(label, destTime)
        { }

        public override string GetAmPmString()
        {
            return DateTime.Today.Add(Time).ToString("tt");
        }

        public override string GetTimeString()
        {
            return Time.ToString(@"hh\:mm");
        }

        public override TimeSpan GetRemainingTime()
        {
            DateTime now = DateTime.Now;
            DateTime dest = DateTime.Today.Add(Time).AddSeconds(1);

            if (now > dest)
                dest = dest.AddDays(1);

            return dest - now;
        }
    }
}
