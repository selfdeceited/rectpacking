using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Strategies
{
    public abstract class StrategyManager:Strategy
    {
        public List<Strategy> UsedStrategies { get; set; }

        public StrategyManager() : base()
        {
        }

        public StrategyManager(params Strategy[] strategies) :base(Algorythm.Hybrid, Complexity.Unknown)
        {
            this.UsedStrategies = new List<Strategy>();
            foreach (var strategy in strategies)
            {
                this.UsedStrategies.Add(strategy);    
            }
            
        }
    }
}
