using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class TimeLine
    {
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Current { get; set; }
        public List<TimeStamp> TimeStamps { get; set; }

        public TimeLine()
        {
            StartDate = DateTime.Now;
            Current = TimeSpan.Zero;
            TimeStamps = new List<TimeStamp>();
        }

        public TimeLine(DateTime StartDate)
        {
            this.StartDate = StartDate;
            TimeStamps = new List<TimeStamp>();
        }

        public void AddInitialTimeStamp()
        {
            this.TimeStamps.Add(new TimeStamp
            {
                Time = this.StartDate,
                State = TimeStampState.InQueue
            });
        }

        public bool NowIsOnTimeStamp()
        {
            return this.TimeStamps.Any(ts => ts.Time == StartDate + Current);
        }

        public void ProlongCurrentToNextTimeStamp()
        {
            var anyNextTimeStamp = TimeStamps.Any(ts => ts.Time > StartDate + Current);
            if (anyNextTimeStamp)
            {
                var nextTimeStamp = TimeStamps.Where(ts => ts.Time > StartDate + Current).OrderBy(ts => ts.Time).First();
                if (nextTimeStamp != null)
                {
                    Current = nextTimeStamp.Time - StartDate;
                }
            }
            
        }
    }
}
