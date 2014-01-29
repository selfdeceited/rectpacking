﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies;
using RectPacking.Strategies.Heuristic;

namespace RectPacking.Tests
{
    [TestFixture]
    public class StrategyTest
    {
        [Test]
        public void StrategyManager()
        {
            //if iteration < 2 -  top right, area > 400, area < 700; 
            //if iteration > 2 - last products with numbers 2, 4 or 6
            var fake1 = new FakeStrategy1();
            //take the last and biggest to lower corners
            var fake2 = new FakeStrategy2();

            var products = new List<Product>();
            for (int i = 1, j = 10; i <= 10; i++, j--)
                products.Add(new Product("product" + i, 5 * i, 5 * j)); 

            //5,50 10,45 15,40 20,35 25,30 30,25 35,20 40,15 45,10 50,5

            var massPlacement = new PlacementProcess(new VibroTable(200, 200), products);
            massPlacement.Proceed(new FakeStrategyManager(fake1, fake2), true);
            //todo: write down all coas as must be
            Assert.True(massPlacement.PlacedCOAs.Any());

            //top right, area > 400, area < 700; 10*45 15*40 40*15 45*10 first is 10*45
            Assert.AreEqual(massPlacement.PlacedCOAs[0].ToString(),"10*45 cTopRight on p200,0");
            Assert.True(massPlacement.PlacedCOAs[0].Product.Area > 400);
            Assert.True(massPlacement.PlacedCOAs[0].Product.Area < 700);
            Assert.True(massPlacement.PlacedCOAs[1].ToString().Contains("cTopRight"));
            Assert.True(massPlacement.PlacedCOAs[1].Product.Area > 400);
            Assert.True(massPlacement.PlacedCOAs[1].Product.Area < 700);

        }
        [Test]
        public void MassPlacement()
        {
            var products = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacement = new PlacementProcess(SampleInitializer.CreateVibroTable(), products);
            massPlacement.Proceed(new EmptyStrategy(), true);
            //must be taken 6 first : 100-200, 300-100, 200-300  50-50, 50-50, 100-50
            Assert.AreEqual(massPlacement.PlacedCOAs.Count, 6);

            var newProducts = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacementWithSimpleHeuristics = new PlacementProcess(SampleInitializer.CreateVibroTable(), newProducts);
            massPlacementWithSimpleHeuristics.Proceed(new SimpleHeuristicStrategy(), true);
            //must be taken 4 : 100-200, 300-100, 200-300  100-50
            Assert.AreEqual(massPlacementWithSimpleHeuristics.PlacedCOAs.Count, 4);
            //todo: write down all coas as must be
        }

        [Test]
        public void CavingHeuristicsTest()
        {
            var newProducts = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacementWithCavingHeuristics = new PlacementProcess(SampleInitializer.CreateVibroTable(), newProducts);
            massPlacementWithCavingHeuristics.Proceed(new QuasiHumanHeuristicStrategy(), true);


            foreach (var sample in massPlacementWithCavingHeuristics.PlacedCOAs)
                foreach (var pretender in massPlacementWithCavingHeuristics.PlacedCOAs)
                    if (sample != pretender)
                        Assert.False(sample.HasIntersectionWith(pretender));


        }
    }
}
