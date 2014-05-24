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
using RectPacking.Strategies.Metaheuristic;

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
                products.Add(new Product(i * 40, "product" + i, 5 * i, 5 * j)); 

            //5,50 10,45 15,40 20,35 25,30 30,25 35,20 40,15 45,10 50,5

            var massPlacement = new DynamicPlacement(new VibroTable(200, 200), products);
            massPlacement.Proceed(new FakeStrategyManager(fake1, fake2), true);

            Assert.False(massPlacement.Left.Any());
            Assert.AreEqual(massPlacement.Done.Count, 10);

            Assert.AreEqual(massPlacement.Done[0].Product.Name, "product1");//because FreezeTime is minimal 5*50
        }
        [Test]
        public void MassPlacement()
        {
            var products = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacement = new StaticPlacement(SampleInitializer.CreateVibroTable(), products);
            massPlacement.Proceed(new EmptyStrategy(), true);
            //must be taken 6 first : 100-200, 300-100, 200-300  50-50, 50-50, 100-50


            var newProducts = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacementWithSimpleHeuristics = new StaticPlacement(SampleInitializer.CreateVibroTable(), newProducts);
            massPlacementWithSimpleHeuristics.Proceed(new SimpleHeuristicStrategy(), true);
            //must be taken 4 : 100-200, 300-100, 200-300  100-50

            //todo: write down all coas as must be
        }

        [Test]
        public void CavingHeuristicsTest()
        {
            var newProducts = SampleInitializer.CreateProductsForMassPlacement();
            var massPlacementWithCavingHeuristics = new StaticPlacement(SampleInitializer.CreateVibroTable(), newProducts);
            massPlacementWithCavingHeuristics.Proceed(new QuasiHumanHeuristicStrategy(), true);

            foreach (var sample in massPlacementWithCavingHeuristics.OnTable)
                foreach (var pretender in massPlacementWithCavingHeuristics.OnTable)
                    if (sample != pretender)
                        Assert.False(sample.HasIntersectionWith(pretender));


        }


        [Test]
        public void ComplexPlacementTest()
        {
            var table = new VibroTable(400, 300);
            var products = SampleInitializer.CreateRandomProdicts(50);
            var simple = new SimplePlacementProcess(table, products);
            var coaPlacement = new StaticPlacement(table, products);
            var complexPlacement = new ComplexPlacementProcess(table, products, simple, coaPlacement);
            complexPlacement.Proceed(new EmptyStrategy(), true);

        }

        [Test]
        public void MultiTable()
        {
            var room = new Room(500, 500);
            room.PlaceTable(new VibroTable(100,200), 0, 0);
            room.PlaceTable(new VibroTable(200, 200), 300, 300);
            var products = SampleInitializer.CreateRandomProdicts(180);
            var placement = new StaticPlacement(room, products);
            placement.ProceedRoom(new EmptyStrategy(), true);
        }
    }
}
