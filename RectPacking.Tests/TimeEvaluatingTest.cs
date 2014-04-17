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
                new Product(120, "120min", 100, 100),
                new Product(100, "100min", 50, 100),
                new Product(70, "70min", 100, 50),
                new Product(50, "50min", 50, 50),
                new Product(20, "20min", 50, 30),

                new Product(60, "60min", 20, 40),
                new Product(30, "30min", 50, 60),
                new Product(45, "45min", 70, 30),
                new Product(25, "25min", 50, 100),
                new Product(10, "10min", 80, 30),

                new Product(200, "200min", 50, 50),
                new Product(150, "150min", 50, 100),
                new Product(120, "120min", 70, 30),

            };
            var process = new PlacementProcess(new VibroTable(150, 150), productList);
            process.Proceed(new SimpleHeuristicStrategy(), true);

        }
    }
}
