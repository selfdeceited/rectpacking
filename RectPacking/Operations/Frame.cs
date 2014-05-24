using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RectPacking.Models;

namespace RectPacking.Operations
{
    public class Frame
    {
        
        public VibroTable VibroTable { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

        public List<COA> SetList { get; set; }

        [JsonIgnore]
        public DateTime StartDate { get; set; }

        [JsonIgnore]
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public double CoveredArea { get; set; }

        public double CoveredPercentage { get; set; }

        [JsonIgnore]
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
