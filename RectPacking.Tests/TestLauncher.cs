using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies.Heuristic;
using RectPacking.Strategies.Metaheuristic;

namespace RectPacking.Tests
{
    [TestFixture]
    public class TestLauncher
    {
        [Test]
        public void HighLoadPlacement()
        {
            var products = new List<Product>();
            for (int i = 0; i < 120; i++)
            {
                var randX = new Random((int)(DateTime.Now.ToBinary() / 215 - i));
                var randY = new Random((int)(1234 + DateTime.Now.Millisecond + 3 * i));
                var randomX = randX.Next(20, 90);
                var randomY = randY.Next(10, 70);
                products.Add(new Product("product" + i, randomX, randomY));
            }
            var massPlacement = new PlacementProcess(new VibroTable(400, 400), products);
            massPlacement.Proceed(new SimpleHeuristicStrategy(), true, "simple");

            //no overlapping
            foreach (var sample in massPlacement.PlacedCOAs)
                foreach (var pretender in massPlacement.PlacedCOAs)
                    if (sample != pretender)
                        Assert.False(sample.HasIntersectionWith(pretender));
        }

        [Test]
        public void CavingHighLoadPlacement()
        {
            var products = new List<Product>();
            for (int i = 0; i < 120; i++)
            {
                var randX = new Random((int)(DateTime.Now.ToBinary() / 215 - i));
                var randY = new Random((int)(1234 + DateTime.Now.Millisecond + 3 * i));
                var randomX = randX.Next(20, 90);
                var randomY = randY.Next(10, 70);
                products.Add(new Product("product" + i, randomX, randomY));
            }
            var massPlacement = new PlacementProcess(new VibroTable(400, 400), products);
            massPlacement.Proceed(new QuasiHumanHeuristicStrategy(), true, "caving");

            //no overlapping
            foreach (var sample in massPlacement.PlacedCOAs)
                foreach (var pretender in massPlacement.PlacedCOAs)
                    if (sample != pretender)
                        Assert.False(sample.HasIntersectionWith(pretender));
        }
        [Test]
        public void MetaHeuristicsHighLoadPlacement()
        {
            var products = new List<Product>();
            for (int i = 0; i < 120; i++)
            {
                var randX = new Random((int)(DateTime.Now.ToBinary() / 215 - i));
                var randY = new Random((int)(1234 + DateTime.Now.Millisecond + 3 * i));
                var randomX = randX.Next(20, 90);
                var randomY = randY.Next(10, 70);
                products.Add(new Product("product" + i, randomX, randomY));
            }
            var massPlacement = new PlacementProcess(new VibroTable(400, 400), products);

            var maxStr = new Strategies.Primitive.MaxAreaFirstHeuristicStrategy();
            var smallStr = new Strategies.Primitive.SmallFirstHeuristicStrategy();
            var toCorners = new Strategies.Primitive.CloserToCornersHeuristicStrategy();
            massPlacement.Proceed(new SimpleMetaheuristicStrategy(maxStr,toCorners, smallStr), true, "meta");

            //no overlapping
            foreach (var sample in massPlacement.PlacedCOAs)
                foreach (var pretender in massPlacement.PlacedCOAs)
                    if (sample != pretender)
                        Assert.False(sample.HasIntersectionWith(pretender));
        }
    }
}
