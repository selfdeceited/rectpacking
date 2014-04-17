using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class TimeStamp
    {
        public DateTime Time { get; set; }
        public TimeStampState State { get; set; }
        public Product Product { get; set; }

        public TimeStamp(Product Product)
        {
            this.Time = Product.PlacedOnTable + Product.FreezeTime;
            this.Product = Product;
            this.State = TimeStampState.IsFurther;
        }

        private TimeStamp(DateTime Time)//for initial
        {
            this.Time = Time;
            this.State = TimeStampState.InQueue;
        }

        public TimeStamp()
        {   }

        public static TimeStamp CreateInitialTimeStampFor(TimeLine timeLine)
        {
            return new TimeStamp(timeLine.StartDate);
        }

        public void HandleState()
        {

        }
    }

    public enum TimeStampState
    {
        IsFurther,
        InQueue,
        Initialized
    }
}
