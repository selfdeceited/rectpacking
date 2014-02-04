using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Operations
{
    public class IterationStat
    {
        public int Iteration { get; set; }
        public List<COA> Left { get; set; }
        public List<COA> Placed { get; set; }
        public long TotalArea { get; set; }
        public long PlacedArea { get; set; }
        public decimal Percentage { get; set; }

        public IterationStat(PlacementProcess placement, int Iteration)
        {
            this.Left = placement.Left;
            this.Placed = placement.Placed;
            this.TotalArea = placement.VibroTable.Area;
            this.PlacedArea = placement.Placed.Sum(coa => coa.Product.Area);
            this.Iteration = Iteration;
            this.Percentage = this.PlacedArea/this.TotalArea*100;
        }

        public IterationStat()
        {

        }
    }
}
