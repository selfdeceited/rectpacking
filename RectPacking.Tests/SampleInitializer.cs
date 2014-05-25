using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Tests
{
    public static class SampleInitializer
    {
        public static List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product("Sample 1", 100, 50),
                new Product("Sample 2", 200, 100)
            };
        }
        public static List<Product> CreateProductsForMassPlacement()
        {
            return new List<Product>
            {
                new Product(100,200),
                new Product(300,100),
                new Product(200,300),
                new Product(50,50),
                new Product(50,50),
                new Product(100,50),
                new Product(100,100),
            };
        }

        public static VibroTable CreateVibroTable()
        {
            return new VibroTable(400, 300);
        }

        public static List<Product> CreateRandomProdicts(int count, bool withTime = false)
        {
            var products = new List<Product>();
            for (int i = 0; i < count; i++)
            {
                var randX = new Random((int)(DateTime.Now.ToBinary() / 215 - i));
                var randY = new Random((int)(1234 + DateTime.Now.Millisecond + 3 * i));
                var randTime = new Random((int)(i*DateTime.Now.Ticks) + i.ToString().GetHashCode());

                var randomX = randX.Next(20, 90);
                var randomY = randY.Next(10, 70);
                var randomTime = randTime.Next(40, 400);
                randomTime = CeilingByDegree(randomTime, 30);
                products.Add(withTime
                    ? new Product(randomTime, "product" + i, randomX, randomY)
                    : new Product("product" + i, randomX, randomY));
            }
            return products;
        }

        public static List<Product> CreateProductsWithTime(int count)
        {
            return CreateRandomProdicts(count, true);
        }

        public static int CeilingByDegree(int random, int degree)
        {
            random = random % degree == 0 ? random : random + (degree - random % degree);
            return random;
        }
    }
}
