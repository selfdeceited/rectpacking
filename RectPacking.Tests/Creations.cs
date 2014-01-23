using System;
using System.Collections.Generic;
using NUnit.Framework;
using RectPacking.Models;
namespace RectPacking.Tests
{
    [TestFixture]
    public class Creations
    {
        [Test]
        public void CreateVibroTable()
        {
            var smallTable = new VibroTable(200, 300);
            Assert.True(smallTable.IsValid);
            Assert.AreEqual(smallTable.Area, 60000);

            var notExistsTable1 = new VibroTable(200, 0);
            Assert.False(notExistsTable1.IsValid);

            var notExistsTable2 = new VibroTable(0, -50);
            Assert.False(notExistsTable2.IsValid);
        }
        [Test]
        public void CreateProduct()
        {
            var invalidProduct = new Product("Invalid One", -500, 0);
            Assert.False(invalidProduct.IsValid);

            var rotatedProduct = new Product("Pillar", 100, 500);
            Assert.True(rotatedProduct.IsValid);
            rotatedProduct.Rotate();
            Assert.True(rotatedProduct.IsValid);
            Assert.AreEqual(rotatedProduct.Width, 500);
            Assert.AreEqual(rotatedProduct.Height, 100);

            var stair = new Product("Stair", 100, 200);
            Assert.True(stair.CanContainInArea(300, 300));
            Assert.True(stair.CanContainInArea(200, 200));

            Assert.True(stair.CanContainInArea(200, 100));
            Assert.True(stair.CanContainInArea(100, 200));

            Assert.False(stair.CanContainInArea(100, 100));
            

            var averageTable = new VibroTable(300, 300);
            Assert.True(stair.CanContainIn(averageTable));

            var smallTable = new VibroTable(200, 300);
            Assert.True(stair.CanContainIn(smallTable));

            var littleTable = new VibroTable(200, 100);
            Assert.True(stair.CanContainIn(littleTable));

            var tinyTable = new VibroTable(100, 100);
            Assert.False(stair.CanContainIn(tinyTable));

            
        }

        [Test]
        public void CreatePoint()
        {
            var invalid = new Point(-5, 0, true);
            Assert.False(invalid.IsValid);
            var table = new VibroTable(100, 200);
            var point = new Point(10, 10, true);
            Assert.True(point.IsValid);
            Assert.True(point.IsWithin(table));
            var badPoint1 = new Point(300, -5, true);
            Assert.False(badPoint1.IsWithin(table));
            var badPoint2 = new Point(300, 50, true);
            Assert.False(badPoint2.IsWithin(table));
            var badPoint3 = new Point(500, 200, true);
            Assert.False(badPoint3.IsWithin(table));
            var goodPoint1 = new Point(0, 0, true);
            Assert.True(goodPoint1.IsWithin(table));
            var goodPoint2 = new Point(100, 200, true);
            Assert.True(goodPoint2.IsWithin(table));
            var goodPoint3 = new Point(50, 200, true);
            Assert.True(goodPoint3.IsWithin(table));
            var goodPoint4 = new Point(100, 0, true);
            Assert.True(goodPoint4.IsWithin(table));

            var lastPoint = new Point(200, 200, true);
            Assert.True(lastPoint.IsWithinIncludedArea(200, 200, 200, 200));
            Assert.False(lastPoint.IsWithinArea(30, 200, 100, 300));
        }

        [Test]
        public void CreateCOA()
        {
            //to create COA, you need a main point
            var stair = new Product("Stair", 100, 200);
            var notMainPoint = new Point(10, 10);
            var invalidCOA = new COA(stair, notMainPoint, COA.CornerType.TopRight, true); //product, corner, is rotated
            Assert.False(invalidCOA.IsValid);

            var mainPoint = new Point(10, 10, true);
            var validCOA = new COA(stair, mainPoint, COA.CornerType.TopRight, true);
            Assert.True(validCOA.IsValid);
            var COAs = new List<COA>();
            COA.ToPack(stair, mainPoint, ref COAs);
            foreach (var coa in COAs)
                Assert.True(coa.IsValid);

            Assert.AreEqual(COAs.Count, 8);
            //  TODO: MORE THOROUGH TESTS
        }

        [Test]
        public void RotatingProductWhileCreatingCOA()
        {
            var point = new Point(50, 50, true);
            //after rotating product is still 30*40
            var someProduct = new Product(50, 80);
            var someCOA = new COA(someProduct, point, COA.CornerType.DownLeft, true);
            //there was a rotate, but product is still, and product in COA is rotated
            Assert.AreEqual(someProduct.Width, 50);
            Assert.AreEqual(someProduct.Height, 80);
        }

        [Test]
        public void CreatingPointsInCOA()
        {
            //static one
            var point = new Point(100, 90, true);
            var pList = COA.CalculatePoints(COA.CornerType.TopRight, point, new Product(20, 20));
            Assert.AreEqual(pList[0].X, 100); 
            Assert.AreEqual(pList[0].Y, 90); //main point

            Assert.AreEqual(pList[1].X, 100);//lower
            Assert.AreEqual(pList[1].Y, 110);

            Assert.AreEqual(pList[2].X, 80);//lefter
            Assert.AreEqual(pList[2].Y, 90);

            Assert.AreEqual(pList[3].X, 80);//opposite
            Assert.AreEqual(pList[3].Y, 110);
            
            //coa shell
            var product = new Product(30, 40);
            var topLeft = new COA(product, point, COA.CornerType.TopLeft, false); //not rotated

            Assert.AreEqual(topLeft.Points[0].X, 100);
            Assert.AreEqual(topLeft.Points[0].Y, 90); //main point

            Assert.AreEqual(topLeft.Points[1].X, 100);//lower
            Assert.AreEqual(topLeft.Points[1].Y, 130);

            Assert.AreEqual(topLeft.Points[2].X, 130);//righter
            Assert.AreEqual(topLeft.Points[2].Y, 90);

            Assert.AreEqual(topLeft.Points[3].X, 130);//opposite
            Assert.AreEqual(topLeft.Points[3].Y, 130);

            var topLeftR = new COA(product, point, COA.CornerType.TopLeft, true); // rotated

            Assert.AreEqual(topLeftR.Points[0].X, 100);
            Assert.AreEqual(topLeftR.Points[0].Y, 90); //main point

            Assert.AreEqual(topLeftR.Points[1].X, 100);//lower
            Assert.AreEqual(topLeftR.Points[1].Y, 120);

            Assert.AreEqual(topLeftR.Points[2].X, 140);//righter
            Assert.AreEqual(topLeftR.Points[2].Y, 90);

            Assert.AreEqual(topLeftR.Points[3].X, 140);//opposite
            Assert.AreEqual(topLeftR.Points[3].Y, 120);

           
            var downRightR = new COA(product, point, COA.CornerType.DownRight, true);
            //40-30
            Assert.AreEqual(downRightR.Points[0].X, 100);
            Assert.AreEqual(downRightR.Points[0].Y, 90); //main point

            Assert.AreEqual(downRightR.Points[1].X, 100);//higher
            Assert.AreEqual(downRightR.Points[1].Y, 60);

            Assert.AreEqual(downRightR.Points[2].X, 60);//lefter
            Assert.AreEqual(downRightR.Points[2].Y, 90);

            Assert.AreEqual(downRightR.Points[3].X, 60);//opposite
            Assert.AreEqual(downRightR.Points[3].Y, 60);
        }

        [Test]
        public void TouchTest()
        {
            var coa1 = new COA(new Product(200, 300), new Point(0, 0, true), COA.CornerType.TopLeft, false);
            var coa2 = new COA(new Product(200, 300), new Point(0, 300, true), COA.CornerType.TopLeft, false);
            Assert.True(coa1.Touches(coa2));
            Assert.True(coa2.Touches(coa1));
            var coa3 = new COA(new Product(300, 300), new Point(0, 300, true), COA.CornerType.TopLeft, false);
            Assert.True(coa3.Touches(coa1));
            Assert.True(coa1.Touches(coa3));
            var coa4 = new COA(new Product(200, 300), new Point(100, 0, true), COA.CornerType.TopLeft, false);
            Assert.True(coa3.Touches(coa4));
            Assert.True(coa4.Touches(coa3));
            var coa5 = new COA(new Product(400, 300), new Point(0, 300, true), COA.CornerType.TopLeft, false);
            Assert.True(coa5.Touches(coa4));
            Assert.True(coa4.Touches(coa5));

            Assert.True(coa2.Touches(coa4));
            Assert.True(coa4.Touches(coa2));
            var coa6 = new COA(new Product(200, 300), new Point(200, 300, true), COA.CornerType.TopLeft, false);
            Assert.True(coa4.Touches(coa6));
            Assert.True(coa6.Touches(coa4));

            var coa7 = new COA(new Product(400, 300), new Point(400, 0, true), COA.CornerType.TopLeft, false);
            Assert.False(coa1.Touches(coa7));
            Assert.False(coa7.Touches(coa1));
            var coa8 = new COA(new Product(50, 50), new Point(500, 500, true), COA.CornerType.TopLeft, false);
            Assert.False(coa1.Touches(coa8));
            Assert.False(coa8.Touches(coa1));

            //one-point

            Assert.False(coa1.Touches(coa6));
            Assert.False(coa6.Touches(coa1));

            //withTable

        }
    }
}
