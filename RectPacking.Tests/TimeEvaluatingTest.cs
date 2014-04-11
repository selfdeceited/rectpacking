using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies.Heuristic;

namespace RectPacking.Tests
{
    [TestFixture]
    class TimeEvaluatingTest
    {
        [Test]
        public void SimpleTimeTest()
        {
            var productList = new List<Product>
            {
                new Product(300, "60min", 100, 100),
                new Product(200, "30min", 50, 100),
                new Product(250, "45min", 100, 50),
                new Product(50, "25min", 50, 50),
                new Product(20, "10min", 50, 30),
            };
            var process = new PlacementProcess(new VibroTable(150, 150), productList);
            process.Proceed(new SimpleHeuristicStrategy(), true);

        }
    }
}
