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
            // TODO 설정에 따라서 시를 보여줄지 말지 선택하게 하기
            return GetRemainingTime().ToString("hh:mm:ss");
        }
    }
}
