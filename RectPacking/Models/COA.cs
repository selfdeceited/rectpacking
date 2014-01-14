using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Models
{
    public class COA
    {
        public Product Product { get; set; }
        public Point MainPoint { get; set; }
        public List<Point> Points { get; set; }

        public enum CornerType
        {
            TopRight, TopLeft, DownRight, DownLeft
        }

        public CornerType Corner { get; set; }
        public bool IsRotated { get; set; }
        public bool IsValid { get; set; }
        public COA(Product Product, Point MainPoint, CornerType CornerType, bool IsRotated)
        {
            this.Product = Product;
            this.MainPoint = MainPoint;
            this.Corner = CornerType;
            this.IsRotated = IsRotated;
            this.IsValid = Product != null && MainPoint != null && MainPoint.IsMain;
            this.Points = CalculatePoints(CornerType, MainPoint, Product);
        }

        public static void ToPack(Product Product, Point Point, ref List<COA> list)
        {
            var corners = (CornerType[])Enum.GetValues(typeof (CornerType));
            foreach (var corner in corners)
            {
                //TODO: refactor!
                list.Add(new COA(Product, Point, corner, false)); 
                list.Add(new COA(Product, Point, corner, true));
            }
        }

        public List<Point> CalculatePoints(CornerType CornerType, Point MainPoint, Product Product)
        {
            var points = new List<Point>();
            points.Add(MainPoint);
            var corner = CornerType.ToString();
            var oppositeX = corner.Contains("Left") ? MainPoint.X + Product.Width : MainPoint.X - Product.Width;
            var oppositeY = corner.Contains("Top") ? MainPoint.Y + Product.Height : MainPoint.Y - Product.Height;
            points.Add(new Point(MainPoint.X, oppositeY, false));
            points.Add(new Point(oppositeX, MainPoint.Y, false));
            points.Add(new Point(oppositeX, oppositeY, false));

            return points;
        }

        public void Place()
        {

        }
    }
}
