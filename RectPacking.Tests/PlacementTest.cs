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
            placement.CreateCOAs();
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
