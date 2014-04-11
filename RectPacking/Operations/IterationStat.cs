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
        public List<COA> OnTable { get; set; }
        public List<COA> Done { get; set; }
        public long TotalArea { get; set; }
        public long OnTableArea { get; set; }
        public decimal Percentage { get; set; }

        public IterationStat(PlacementProcess placement, int Iteration)
        {
            this.Left = placement.Left;
            this.OnTable = placement.OnTable;
            this.TotalArea = placement.VibroTable.Area;
            this.OnTableArea = placement.OnTable.Sum(coa => coa.Product.Area);
            this.Iteration = Iteration;
            this.Percentage = this.OnTableArea / this.TotalArea * 100;
        }

        public IterationStat()
        {

        }
    }
}
