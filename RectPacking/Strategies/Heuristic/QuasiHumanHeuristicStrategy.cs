using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Helpers;

namespace RectPacking.Strategies.Heuristic
{
    public class QuasiHumanHeuristicStrategy: Strategy
    {
        public List<Caving> Cavings { get; set; }

        public QuasiHumanHeuristicStrategy()
            : base(Algorythm.Heuristic, Complexity.Unknown)
        {
            Cavings = new List<Caving>();
        }
        public override IAction Solve(List<COA> COAs)
        {
            var best = Cavings
                .OrderBy(c => c.Left * c.Left + c.Top * c.Top)
                .ThenBy(c => c.Degree)
                .ThenByDescending(c => c.EdgeIndex)
                .First();
            return best;
        }

        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.IterationStat = new IterationStat(placement, i);
            this.Cavings = GenerateCavingList(placement);
        }

        private List<Caving> GenerateCavingList(PlacementProcess placement)
        {
            var cavings = placement.Left.Select(coa => new Caving(coa, placement)).ToList();
            return cavings;
        }
    }
}
