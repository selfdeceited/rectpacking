using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class COA
    {
        public Product Product { get; set; }
        public Point Point { get; set; }
        public string Corner { get; set; }
        public bool IsRotated { get; set; }
        public bool IsValid { get; set; }
        public COA(Product Product, Point Point, string Corner, bool IsRotated)
        {
            this.Product = Product;
            this.Point = Point;
            this.Corner = Corner;
            this.IsRotated = IsRotated;
            this.IsValid = Product != null && Point != null && Point.IsMain ;
        }

        public static List<COA> ToPack(Product Product, Point Point)
        {
            var list = new List<COA>();
            var corners = new[] {"TopRight", "TopLeft", "DownRight", "DownLeft"};
            foreach (var corner in corners)
            {
                list.Add(new COA(Product, Point, corner, false));
                list.Add(new COA(Product, Point, corner, true));
            }
            return list;
        }

        public void Place()
        {

        }
    }
}
