using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies
{
    public class EmptyStrategy:Strategy
    {
        public EmptyStrategy() : base()
        {
        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
           
        }

        public override IAction Solve(List<COA> COAs)
        {
            return COAs.First();
        }
    }
}
