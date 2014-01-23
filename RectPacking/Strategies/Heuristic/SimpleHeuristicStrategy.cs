using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.Heuristic
{
    public class SimpleHeuristicStrategy: Strategy
    {
        public SimpleHeuristicStrategy(): base(Algorythm.Heuristic, Complexity.Unknown)
        {

        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
        }

        public override IAction Solve(List<COA> COAs )
        {
            if (!COAs.Any()) return null;
            var orderedByArea = COAs.OrderByDescending(coa => coa.Product.Area); //max area is the best.. for now
            var closerToTopLeft = orderedByArea.OrderBy(coa => coa.Points.Min(p => p.X) * coa.Points.Min(p => p.X)
                + coa.Points.Min(p => p.Y) * coa.Points.Min(p => p.Y));
            var best = closerToTopLeft.First();
            return best; 
        }
    }
}
