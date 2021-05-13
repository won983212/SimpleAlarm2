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
            : base(label, destTime, SimpleAlarm2.AlertType.Alarm)
        { }

        public override void OnRootTimerTick()
        {
            DateTime now = DateTime.Now;
            DateTime dest = DateTime.Today.Add(Time).AddSeconds(1);

            if (now > dest)
                dest = dest.AddDays(1);

            RemainingTime = dest - now;
        }
    }
}
