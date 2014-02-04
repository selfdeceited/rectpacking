using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies;

namespace RectPacking.Tests
{
    class FakeStrategyManager:StrategyManager
    {
        public FakeStrategyManager(params Strategy[] strategies)
            : base(strategies)
        {
        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.IterationStat = new IterationStat(placement, i);
        }

        public override IAction Solve(List<COA> COAs)
        {
            var iteration = this.IterationStat.Iteration;
            var strategy1 = this.UsedStrategies.ElementAt(0);
            var strategy2 = this.UsedStrategies.ElementAt(1);

            return iteration > 3 ? strategy2.Solve(COAs) : strategy1.Solve(COAs);
        }
    }
}
