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
        public enum CornerType
        {
            TopRight, TopLeft, DownRight, DownLeft
        }

        public Product Product { get; set; }
        public Point MainPoint { get; set; }
        public List<Point> Points { get; set; }
        public CornerType Corner { get; set; }
        public bool Rotated { get; set; }
        public bool IsValid { get; set; }

        public COA(Product Product, Point MainPoint, CornerType CornerType, bool Rotated)
        {
            this.Product = Product.Dublicate();
            // dublication is perfomed to prevent making links to initial product;
            // because of packing COA creation while rotating initial product
            // can leave it rotated when it doesn't need to be rotated
            if (Rotated) this.Product.Rotate();

            this.MainPoint = MainPoint;
            this.Corner = CornerType;
            this.Rotated = Rotated;
            this.IsValid = MainPoint != null && MainPoint.IsMain;
            this.Points = CalculatePoints(CornerType, MainPoint, this.Product);
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

        public void CalculatePoints()
        {
            this.Points = CalculatePoints(this.Corner, this.MainPoint, this.Product);
        }

        public static List<Point> CalculatePoints(CornerType CornerType, Point MainPoint, Product Product)
        {
            var points = new List<Point>();
            points.Add(MainPoint);
            var corner = CornerType.ToString();
            var oppositeX = corner.Contains("Left") ? MainPoint.X + Product.Width : MainPoint.X - Product.Width;
            var oppositeY = corner.Contains("Top") ? MainPoint.Y + Product.Height : MainPoint.Y - Product.Height;
            points.Add(new Point(MainPoint.X, oppositeY));
            points.Add(new Point(oppositeX, MainPoint.Y));
            points.Add(new Point(oppositeX, oppositeY));

            return points;
        }

        public void Place()
        {
            
        }
        public static List<COA> RemoveFromListWithId(int id, List<COA> list)
        {
            var coasToDelete = list.Where(coa => coa.Product.Identifier == id).ToArray();
            for (int i=0; i < coasToDelete.Count() ;i++)
            {
                list.Remove(coasToDelete[i]);
            }
            return list;
        }
    }
}
