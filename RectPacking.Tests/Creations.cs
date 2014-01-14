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
            Assert.True(stair.CanContainInArea(200, 200));

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
            var notMainPoint = new Point(10, 10, false);
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
    }
}
