using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    public class Alarm : AlertContent
    {
        /*public Alarm() 
            : base("Test", TimeSpan.FromMinutes(35)) 
        { }*/

        public Alarm(string label, TimeSpan destTime)
            : base(label, destTime)
        { }

        public override TimeSpan GetRemainingTime()
        {
            DateTime now = DateTime.Now;
            DateTime dest = new DateTime(now.Year, now.Month, now.Day, Time.Hours, Time.Minutes, Time.Seconds + 1);

            if (now > dest)
                dest = dest.AddDays(1);

            return dest - now;
        }
    }
}
