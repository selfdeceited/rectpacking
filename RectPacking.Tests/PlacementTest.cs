using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RectPacking.Models;
using RectPacking.Operations;
namespace RectPacking.Tests
{
    [TestFixture]
    public class PlacementTest
    {
        [Test]
        public void CreatePlacement()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                SampleInitializer.CreateProducts());
            Assert.AreEqual(placement.MainPoints.Count, 4);
            //points must be:             0,0        400,0        0,300        400,300 
            Assert.True(placement.MainPoints[0].X == 0   && placement.MainPoints[0].Y == 0  );
            Assert.True(placement.MainPoints[1].X == 400 && placement.MainPoints[1].Y == 0  );
            Assert.True(placement.MainPoints[2].X == 0   && placement.MainPoints[2].Y == 300);
            Assert.True(placement.MainPoints[3].X == 400 && placement.MainPoints[3].Y == 300);
            //products Sample1 and Sample2 are there
            Assert.AreEqual(placement.ProductList[0].Name, "Sample 1");
            Assert.AreEqual(placement.ProductList[1].Name, "Sample 2");
            Assert.AreEqual(placement.ProductList.Count, 2);
            //there is a vibroTable - 400x300
            Assert.AreEqual(placement.VibroTable.Width,  400);
            Assert.AreEqual(placement.VibroTable.Height, 300);
        }

        [Test]
        public void CorrectCreateCOAs()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                SampleInitializer.CreateProducts());
            var points = new List<Point> {new Point(50, 50, true)};
            placement.CreateCOAsForPoints(points, false);

            Assert.NotNull(placement.COAs);
            const int numberOfCOAs = 16;
            Assert.AreEqual(placement.COAs.Count, numberOfCOAs);         // 8 positions * 2 products = 32
            Assert.AreEqual(placement.COAs.Count(coa => coa.IsValid), numberOfCOAs); //all are valid
            
            Assert.AreEqual(placement.COAs.Count(coa => coa.IsRotated), numberOfCOAs / 2); //half of them are rotated
            Assert.AreEqual(placement.COAs.Count(coa => coa.Product.Name == "Sample 1"), numberOfCOAs / 2); //half of them are of Sample 1
            Assert.AreEqual(placement.COAs.Count(coa => coa.Product.Name == "Sample 2"), numberOfCOAs / 2); //half of them are of Sample 2
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.TopLeft), numberOfCOAs / 4); //each fourth has the main point in top left corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.TopLeft), numberOfCOAs / 4); //in top right corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.DownLeft), numberOfCOAs / 4); //down left corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.DownRight), numberOfCOAs / 4); //down right corner
            //now some of the COAs as is
            Assert.AreEqual(placement.COAs.First().Corner, COA.CornerType.TopRight);
            Assert.False(placement.COAs.First().IsRotated);

        }

        [Test]
        public void CorrectCOACollisionWithTable()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                new List<Product>{new Product("Sample 3", 350, 50)});
            placement.CreateCOAsForPoints();
        }

        [Test]
        public void ProceedPlacement()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                SampleInitializer.CreateProducts());
            placement.Proceed();
        }

    }
}
