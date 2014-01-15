using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RectPacking.Models;
using RectPacking.Helpers;

namespace RectPacking.Operations
{
    public class PlacementProcess
    {
        public VibroTable VibroTable { get; set; }
        public List<Product> ProductList { get; set; }
        public List<Point> MainPoints { get; set; }
        public string ProcessState { get; set; }
        public List<COA> COAs { get; set; }
        public List<COA> PlacedCOAs { get; set; }

        public PlacementProcess(VibroTable vibroTable, List<Product> productList )
        {
            this.VibroTable = vibroTable;
            this.ProductList = productList;
            this.ProcessState = "Initialized";//todo: work through watching state like that
            //can be added event handlers everytime process state is changed to perform logs
            this.MainPoints = new List<Point>();
            this.COAs = new List<COA>();
            this.PlacedCOAs = new List<COA>();
            this.CreateInitialMainPoints(vibroTable);
        }

        public void CreateInitialMainPoints(VibroTable vibroTable)
        {
            this.MainPoints.Add(new Point(0, 0, true));
            this.MainPoints.Add(new Point(vibroTable.Width, 0, true));
            this.MainPoints.Add(new Point(0, vibroTable.Height, true));
            this.MainPoints.Add(new Point(vibroTable.Width, vibroTable.Height, true));
        }

        public void Proceed()
        {
            var newPoints = this.MainPoints;

            var best = SampleBestCOA();//sample is used to get into the cycle
            while (ResumePlacement(best))
            {
                CreateCOAsForPoints(newPoints);
                best = ChooseBestCOA();
                if (best != null)
                {
                    PlacedCOAs.Add(best);
                    best.Place();//todo: Graphical on-line stuff would be cool
                    newPoints = ManagePointsFor(best); //change link
                    DeleteCOAsWith(best.Product);
                }
            }
            Export.ToJson(PlacedCOAs);
        }

        public bool ResumePlacement(COA best)
        {
            return best!=null;//todo: Stopping criteria?
        }

        public void CreateCOAsForPoints(List<Point> points = null, bool withConstrains = true)
        {
            if (points == null) 
                points = this.MainPoints;
            List<COA> list = this.COAs;
            foreach (var point in points)//because points can differ
            {
                foreach (var product in this.ProductList)
                {
                    COA.ToPack(product, point, ref list);
                }
            }
            if (withConstrains)
                list = Filters.FilterCOAs(this);
            this.COAs = list;
        }

        public COA ChooseBestCOA()
        {
            if (!COAs.Any()) return null;
            return COAs.OrderBy(coa => coa.Product.Area).First(); //max area is the best.. for now
        }

        public List<Point> ManagePointsFor(COA best)
        {
            return this.MainPoints;
        }
        public void DeleteCOAsWith(Product bestProduct)
        {
            this.ProductList = Product.RemoveFromListWithId(bestProduct.Identifier, this.ProductList);
            this.COAs = COA.RemoveFromListWithId(bestProduct.Identifier, this.COAs);
        }

        public COA SampleBestCOA()
        {
            const int greatNumber = 1000000;
            return new COA(new Product(greatNumber, greatNumber, false),
                new Point(), COA.CornerType.TopLeft, false);
        }

    }
}
