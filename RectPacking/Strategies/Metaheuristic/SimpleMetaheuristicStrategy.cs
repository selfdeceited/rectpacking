using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Extensions;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.Metaheuristic
{
    public class SimpleMetaheuristicStrategy: StrategyManager
    {
       // public double SuccessIndex { get; set; }

        public SimpleMetaheuristicStrategy(params Strategy[] strategies)
            : base(strategies)
        {

        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            //this.SuccessIndex = this.IterationStat != null ? this.IterationStat. : ;
            this.IterationStat = new IterationStat(placement, i);
            
        }

        public override IAction Solve(List<COA> COAs)
        {
           // var auxiliaryStrategy = this.UsedStrategies[1];
           // var uselessStratefy = this.UsedStrategies[2];

            Strategy mainStrategy = new EmptyStrategy();
            if (this.UsedStrategies.Count < 3) return mainStrategy.Solve(COAs);

            if (this.IterationStat.Percentage < 30) mainStrategy = this.UsedStrategies[0];
            if (this.IterationStat.Percentage.IsBetweenIncl(30, 70)) mainStrategy = this.UsedStrategies[1];
            if (this.IterationStat.Percentage > 40) mainStrategy = this.UsedStrategies[2];

            return mainStrategy.Solve(COAs);
        }
    }
}
