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
    }
}
