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
    public class PlacementProcess: AbstractPlacement
    {
        public PlacementProcess(VibroTable vibroTable, List<Product> productList, DateTime startDate)
            : base(vibroTable, productList, startDate)
        {
        }

        public PlacementProcess(VibroTable vibroTable, List<Product> productList)
            : base(vibroTable, productList)
        {
            this.Left = ToCOAList(new List<IAction>(base.Left));
            this.Done = ToCOAList(base.Done);
            this.OnTable = ToCOAList(base.OnTable);
            this.MainPoints = new List<Point>();
            this.CreateInitialMainPoints(vibroTable);
        }

        public List<Point> MainPoints { get; set; }
        public new List<COA> Left { get; set; }
        public new List<COA> OnTable { get; set; }
        public new List<COA> Done { get; set; }
        public StringBuilder JSON { get; set; }

        public override void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {
            
        }

        public override void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false)
        {
            
        }

        public List<COA> ToCOAList(List<IAction> initial)
        {
            return initial.Cast<COA>().ToList();
        }

        public void CreateInitialMainPoints(VibroTable vibroTable)
        {
            this.MainPoints.Add(new Point(vibroTable.Left, vibroTable.Top, true));
            this.MainPoints.Add(new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top, true));
            this.MainPoints.Add(new Point(vibroTable.Left, vibroTable.Top + vibroTable.Height, true));
            this.MainPoints.Add(new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top + vibroTable.Height, true));
        }
        public IEnumerable<Point> CreateInitialMainPoints()
        {
            var vibroTable = this.VibroTable;
            yield return (new Point(vibroTable.Left, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left, vibroTable.Top + vibroTable.Height, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top + vibroTable.Height, true));
        }
    }
}
