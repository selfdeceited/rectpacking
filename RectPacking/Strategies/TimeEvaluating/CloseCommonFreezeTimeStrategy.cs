using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Extensions;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Strategies.TimeEvaluating
{
    public  class CloseCommonFreezeTimeStrategy: Strategy
    {
        //Лучший элемент - тот, у которого рейтинг выше
        public CloseCommonFreezeTimeStrategy() : base(Algorythm.Heuristic, Complexity.Unknown)
        {

        }
        public override void UpdateIterationStatFor(int i, PlacementProcess placement)
        {
            this.IterationStat.OnTable = placement.OnTable;
        }

        public override IAction Solve(List<COA> COAs)
        {
            if (!COAs.Any()) return null;
            
            var ratioCOAList = DefineRatiosFor(COAs, this.IterationStat);
            return ratioCOAList.OrderByDescending(r => r.Rating).Select(r => r.Current).First();
        }

        private IEnumerable<RatioCOA> DefineRatiosFor(IEnumerable<COA> coas, IterationStat stat)
        {
            return coas.Select(coa => new RatioCOA(coa, RatioCOA.FindNeighbours(coa, stat.OnTable)));
        }
    }

    public class RatioCOA :  IRated
    {
        
        public double Rating { get; set; }
        public COA Current { get; set; }
        public List<COA> Neighbours { get; set; }

        public RatioCOA(COA COA, List<COA> Neighbours)
        {
            this.Current = COA;
            this.Neighbours = Neighbours;
            this.Rating = CalculateRating();
            
        }

        public static List<COA> FindNeighbours(COA coa, List<COA> onTable)
        {
            return onTable.Where(c => c.Touches(coa)).ToList();
        }

        private double CalculateRating()
        {
            double rating = 0;
            foreach (var neighbour in Neighbours)
            {
                var overlayPercent = RectangleExtension.GetOverlayPercent(Current, neighbour);
                rating += rating + (1 / (1 +
                                      (Math.Abs(Current.Product.FreezeTime.TotalMinutes - neighbour.Product.FreezeTime.TotalMinutes)
                                       / (Current.Product.FreezeTime.TotalMinutes * overlayPercent))));
            }
            return rating;
        }


    }
}
