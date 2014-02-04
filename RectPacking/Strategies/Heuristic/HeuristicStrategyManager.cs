using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.Heuristic
{
    class HeuristicStrategyManager:StrategyManager
    {
        public HeuristicStrategyManager(params Strategy[] strategies)
            : base(strategies)
        {
        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
        }

        public override IAction Solve(List<COA> COAs)
        {
            var thisBest = new COA();
            foreach (var strategy in this.UsedStrategies)
            {
                thisBest = (COA) strategy.Solve(COAs);
            }

            return thisBest;
        }
    }
}
