using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.Primitive
{
    public class MaxAreaFirstHeuristicStrategy:Strategy
    {
        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
        }

        public override IAction Solve(List<COA> COAs)
        {
            return COAs.OrderByDescending(c => c.Product.Area).FirstOrDefault();
        }
    }
}
