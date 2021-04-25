using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAlarm2
{
    // TODO(LATER): 향후 타이머의 Time은 실시간으로 갱신되게 할 것임
    class SpecificTimer : AlertContent
    {
        public SpecificTimer(string label, TimeSpan time)
            : base(label, time)
        { }

        public override TimeSpan GetRemainingTime()
        {
            return Time;
        }
    }
}
