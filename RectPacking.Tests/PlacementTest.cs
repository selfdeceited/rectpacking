using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RectPacking.Helpers;
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
            
            Assert.AreEqual(placement.COAs.Count(coa => coa.Rotated), numberOfCOAs / 2); //half of them are rotated
            Assert.AreEqual(placement.COAs.Count(coa => coa.Product.Name == "Sample 1"), numberOfCOAs / 2); //half of them are of Sample 1
            Assert.AreEqual(placement.COAs.Count(coa => coa.Product.Name == "Sample 2"), numberOfCOAs / 2); //half of them are of Sample 2
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.TopLeft), numberOfCOAs / 4); //each fourth has the main point in top left corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.TopLeft), numberOfCOAs / 4); //in top right corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.DownLeft), numberOfCOAs / 4); //down left corner
            Assert.AreEqual(placement.COAs.Count(coa => coa.Corner == COA.CornerType.DownRight), numberOfCOAs / 4); //down right corner
            //todo: now some of the COAs as is
            Assert.AreEqual(placement.COAs.First().Corner, COA.CornerType.TopRight);
            Assert.False(placement.COAs.First().Rotated);

        }

        [Test]
        public void CorrectCOACollisionWithTable()
        {
            var table = SampleInitializer.CreateVibroTable();

            var usualPlacement = new PlacementProcess(table, 
                new List<Product>{new Product(300, 350)});
            usualPlacement.CreateCOAsForPoints();
            Assert.AreEqual(usualPlacement.COAs.Count, 4);

            var impossibleplacement = new PlacementProcess(table, 
                new List<Product> { new Product(500, 10) });
            impossibleplacement.CreateCOAsForPoints();
            Assert.AreEqual(impossibleplacement.COAs.Count, 0);

            var goodPlacement = new PlacementProcess(table,
                new List<Product> { new Product(20, 200) });
            goodPlacement.CreateCOAsForPoints();
            Assert.AreEqual(goodPlacement.COAs.Count, 8);

            //mass stuff
            var massPlacement = new PlacementProcess(table, new List<Product>
            {
                new Product(20, 200), //8
                new Product(30, 50),  //8
                new Product(600, 500),//0
                new Product(400, 300),//4 (yes, 4)
                new Product(200, 400),//4
            });
            massPlacement.CreateCOAsForPoints();
            Assert.AreEqual(massPlacement.COAs.Count, 24);

        }

        [Test]
        public void ProceedPlacement()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                SampleInitializer.CreateProducts());
            placement.Proceed(true);
            Assert.AreEqual(placement.PlacedCOAs.Count, 2);
            //full placement

        }

        [Test]
        public void ImageCreate()
        {
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(),
                SampleInitializer.CreateProducts());
            var image = new ImageHelper(placement);
            Assert.NotNull(image);
        }
        [Test]
        public void AddCOAToImage()
        {
            var products = SampleInitializer.CreateProducts();
            var placement = new PlacementProcess(SampleInitializer.CreateVibroTable(), products);
            var image = new ImageHelper(placement);
            var toPlace = new COA(products[0], new Point(100, 100, true), COA.CornerType.TopLeft, true);
            image.UpdateStatus(placement, toPlace);
            Assert.NotNull(image);
        }

        [Test]
        public void MassPlacement()
        {
            var products = new List<Product>
            {
                new Product(100,200),
                new Product(300,100),
                new Product(200,300),
                new Product(50,50),
                new Product(50,50),
                new Product(100,50),
                new Product(100,100),
            };
            //must be taken 4 : 100-200, 300-100, 200-300 and 100-00 
            var massPlacement = new PlacementProcess(SampleInitializer.CreateVibroTable(), products);
            massPlacement.Proceed(true);
            Assert.AreEqual(massPlacement.PlacedCOAs.Count, 4);
        }

        [Test]
        public void HighLoadPlacement()
        {
            var products = new List<Product>();
            for (int i = 0; i < 40; i++)
            {
                var randX = new Random((int)(DateTime.Now.ToBinary()/215-i));
                var randY = new Random((int)(1234+DateTime.Now.Millisecond+3*i));
                var randomX = randX.Next(50, 150);
                var randomY = randY.Next(30, 100);
                products.Add(new Product("product" + i, randomX, randomY));
            }
            var massPlacement = new PlacementProcess(new VibroTable(500, 400), products);
            massPlacement.Proceed();
        }

    }
}
