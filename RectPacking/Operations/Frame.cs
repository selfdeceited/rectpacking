using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Operations
{
    public class Frame
    {
        public VibroTable VibroTable { get; set; }
        public int Index { get; set; }
        public List<COA> SetList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double CoveredArea { get; set; }
        public double CoveredPercentage { get; set; }
        public TimeSpan TimeForFrame { get; set; }


        public void GetStats()
        {
            this.TimeForFrame = SetList.Max(c => c.Product.FreezeTime);
            this.EndDate = StartDate.Add(TimeForFrame);
            this.CoveredArea = SetList.Sum(c => c.Product.Area);
            this.CoveredPercentage = CoveredArea / this.VibroTable.Area;
        }
    }
}
