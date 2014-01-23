using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies
{
    public abstract class Strategy
    {
        public enum Algorythm
        {
            Heuristic, MetaHeuristic, SimulatedAnnealing, Hybrid, Other
        }
        public Algorythm AlgorythmType { get; set; }
        public IterationStat IterationStat { get; set; }

        public enum Complexity
        {
            Logarythmic, Linear, SuperPositioned, Polinomial, Exponential, Factorial, Unknown
        }

        public Complexity ComplexityType { get; set; }

        public Strategy(Algorythm AlgorythmType, Complexity ComplexityType)
        {
            this.AlgorythmType = AlgorythmType;
            this.ComplexityType = ComplexityType;
            this.IterationStat = new IterationStat();
        }

        public Strategy()
        {

        }

        public abstract void UpdateIterationStatFor(int i, PlacementProcess placement);

        public abstract IAction Solve(List<COA> COAs);

    }
}
