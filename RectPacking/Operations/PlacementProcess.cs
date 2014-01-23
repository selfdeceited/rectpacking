using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RectPacking.Models;
using RectPacking.Helpers;
using RectPacking.Strategies;

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

        public void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {

            var image = new ImageHelper(this, folderTag);
            var newPoints = this.MainPoints;
            var best = SampleBestCOA();//sample is used to get into the cycle

            var iteration = 0;
            while (ResumePlacement(best))
            {
                iteration++;
                CreateCOAsForPoints(newPoints);
                best = ChooseBestCOA(manager, iteration);
                
                if (best != null)
                {
                    PlacedCOAs.Add(best);
                    if (debug) image.UpdateStatus(this, best);
                    newPoints = ManagePointsFor(best);
                    DeleteCOAsWith(best.Product);
                }
            }
            if (debug) image.UpdateStatus(this);
            var json = Export.ToJson(this);
            var folder = string.IsNullOrEmpty(folderTag) ? "" :  folderTag + "\\";
            var textFile = new System.IO.StreamWriter("C:\\test\\" + folder + "data.json");
            
            textFile.Write(json);
            textFile.Close();

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

        public COA ChooseBestCOA(Strategy manager, int iteration)
        {
            manager.UpdateIterationStatFor(iteration, this);
            if (!COAs.Any()) 
                return null;

            return (COA) manager.Solve(COAs);
        }

        public List<Point> ManagePointsFor(COA best)
        {
            foreach (var point in best.Points)
            {
                if(!point.IsMain)
                point.IsMain = true;
                MainPoints.Add(point);
            }
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

        public void RevertCOA(COA a)
        {
            this.COAs.Remove(a);
        }
    }
}
