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
    }
}
