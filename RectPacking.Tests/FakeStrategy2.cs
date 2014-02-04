using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Tests
{
    class FakeStrategy2: FakeStrategy
    {
        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.IterationStat = new IterationStat(placement, i);
        }

        public override IAction Solve(List<COA> COAs)
        {
            var ordered = COAs.OrderByDescending(coa => coa.Corner == COA.CornerType.DownLeft
                                            || coa.Corner == COA.CornerType.DownRight)
                                            .ThenByDescending(coa => coa.Product.Area)
                                            .ThenByDescending(COAs.IndexOf);
            return ordered.First();
        }
    }
}
