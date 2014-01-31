using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Strategies;

namespace RectPacking.Operations
{
    public class ComplexPlacementProcess:AbstractPlacement
    {
        public List<AbstractPlacement> Placements { get; set; }

        public ComplexPlacementProcess(VibroTable vibroTable, List<Product> productList, params AbstractPlacement[] processes) 
            : base(vibroTable, productList)
        {
            Placements = new List<AbstractPlacement>();
            Placements.AddRange(processes);
        }

        public override void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {
            var initial = Placements[0];
            initial.Proceed(new EmptyStrategy(), debug);
            for (int index = 1; index < Placements.Count; index++)
            {
                Placements[index].ProceedFrom(manager,
                    Placements[index - 1].Placed,
                    Placements[index - 1].Image,
                    Placements[index - 1].Iteration,
                    debug);
            }
        }

        public override void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false)
        {
            
        }
    }
}
