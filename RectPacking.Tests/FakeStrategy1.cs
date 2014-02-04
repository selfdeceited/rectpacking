using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Tests
{
    class FakeStrategy1 : FakeStrategy
    {
        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.IterationStat = new IterationStat(placement, i);
        }

        public override IAction Solve(List<COA> COAs)
        {
            IEnumerable<COA> ordered = null;
            if (this.IterationStat.Iteration <= 2)
            {
                   ordered = COAs.Where(coa => coa.Product.Area > 400 && coa.Product.Area < 700)
                    .OrderByDescending(coa => coa.Corner == COA.CornerType.TopRight);
            }
            else
            {
                ordered = COAs.Where(coa => 
                       coa.Product.Name.Contains("2") 
                    || coa.Product.Name.Contains("4")
                    || coa.Product.Name.Contains("6")).OrderByDescending(coa=>coa.Product.Identifier);
            }
            return ordered.First();
        }
    }
}
