using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.Primitive
{
    public class CloserToCornersHeuristicStrategy:Strategy
    {

        public Point Center { get; set; }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.Center = new Point(placement.VibroTable.Width/2, placement.VibroTable.Height/2);
        }

        public override IAction Solve(List<COA> COAs)
        {
            foreach (var coa in COAs)
            {
                var coaCenter = new Point(coa.Left + coa.Width/2, coa.Top + coa.Height/2);
                coa.Distance = 
                    Math.Sqrt((coaCenter.X - Center.X) * (coaCenter.X - Center.X) +
                              (coaCenter.Y - Center.Y) * (coaCenter.Y - Center.Y));
            }
            return COAs.OrderByDescending(c => c.Distance).FirstOrDefault();
        }
    }
}
