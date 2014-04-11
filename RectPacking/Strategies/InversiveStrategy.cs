using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Strategies
{
    abstract class InversiveStrategy : Strategy
    {
        public void Inverse()
        {
            var coa = this.IterationStat.OnTable.Last();
            IterationStat.Left.Add(coa);
            IterationStat.OnTable.Remove(coa);
            //todo: works with local version, not with placementProcess. think about it.
        }
        public void Inverse(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Inverse();
            }

        }
    }
}
