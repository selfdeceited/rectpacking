using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Operations
{
    public class PlacementProcess
    {
        public VibroTable VibroTable { get; set; }
        public List<Product> ProductList { get; set; }
        public List<Point> Points { get; set; }
        public List<Point> MainPoints { get; set; }
        public List<Bound> Bounds { get; set; }
        public string ProcessState { get; set; }

        public PlacementProcess(VibroTable vibroTable, List<Product> productList )
        {
            this.VibroTable = vibroTable;
            this.ProductList = productList;
            this.ProcessState = "Initialized";
            this.Points = new List<Point>();
            this.MainPoints = new List<Point>();
            this.Bounds = new List<Bound>();
            this.CreateInitialMainPoints(vibroTable);
            this.CreateInitialBounds(vibroTable);
        }

        public void CreateInitialMainPoints(VibroTable vibroTable)
        {
            this.MainPoints.Add(new Point(0, 0, true));
            this.MainPoints.Add(new Point(vibroTable.Width, 0, true));
            this.MainPoints.Add(new Point(0, vibroTable.Height, true));
            this.MainPoints.Add(new Point(vibroTable.Width, vibroTable.Height, true));
        }

        public void CreateInitialBounds(VibroTable vibroTable)
        {
            //do stuff
        }

        public void CreateCOAs()
        {

        }

        public void Proceed()
        {
            CreateCOAs();
            var newPoints = this.MainPoints;
            while (ResumePlacement())
            {
                CreateCOAsForPoints(newPoints);
                COA best = ChooseBestCOA();
                best.Place();
                ManageBoundsFor(best);
                newPoints = ManagePointsFor(best); //change link
                DeleteCOAsWith(best.Product);
            }
        }

        public bool ResumePlacement()
        {
            return false;
        }

        public void CreateCOAsForPoints(List<Point> points)
        {

        }
        public COA ChooseBestCOA()
        {
            return new COA(new Product("1", 1, 1), new Point(1, 1, true), "TopLeft", false);
        }
        public void ManageBoundsFor(COA best)
        {

        }
        public List<Point> ManagePointsFor(COA best)
        {
            return new List<Point>();
        }
        public void DeleteCOAsWith(Product bestProduct)
        {

        }

    }
}
