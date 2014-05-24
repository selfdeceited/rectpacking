using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.TimeEvaluating
{
    public class SlowFirstHeuristicStrategy : Strategy
    {
        public SlowFirstHeuristicStrategy()
            : base(Algorythm.Heuristic, Complexity.Unknown)
        {

        }
        
        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            
        }

        public override IAction Solve(List<COA> COAs)
        {
            if (!COAs.Any()) return null;
            return COAs.OrderByDescending(coa => coa.Product.FreezeTime.TotalMinutes).First();
        }
    }
}
