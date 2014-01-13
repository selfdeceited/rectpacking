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

        public static VibroTable CreateVibroTable()
        {
            return new VibroTable(400, 300);
        }
    }
}
