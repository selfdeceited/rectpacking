using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RectPacking.Extensions;
using RectPacking.Models;

namespace RectPacking.Models
{
    public class COA: IAction
        //COA means Common Orderable Action
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


        //implemented members
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public double Distance { get; set; }


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


            //implemented members in the constructor
            var corner = CornerType.ToString();
            this.Width = this.Product.Width;
            this.Height = this.Product.Height;
            this.Left = corner.Contains("Left") ? MainPoint.X : MainPoint.X - this.Product.Width;
            this.Top = corner.Contains("Top") ? MainPoint.Y : MainPoint.Y - this.Product.Height;
        }

        public COA()
        {
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
            var corner = CornerType.ToString();
            var oppositeX = corner.Contains("Left") ? MainPoint.X + Product.Width : MainPoint.X - Product.Width;
            var oppositeY = corner.Contains("Top") ? MainPoint.Y + Product.Height : MainPoint.Y - Product.Height;
            return new List<Point>
            {
                MainPoint,
                new Point(MainPoint.X, oppositeY),
                new Point(oppositeX, MainPoint.Y),
                new Point(oppositeX, oppositeY)
            };
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

        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Width, Height);
        }

        public bool HasIntersectionWith(COA coa)
        {
            var sample = this.ToRectangle();
            var pretender = coa.ToRectangle();
            if (sample.IntersectsWith(pretender)) return true;
            return false;
        }

        public static bool HasSamePointCoords(COA sample, COA pretender)
        {
            var match = true;
            var sampleRect = sample.ToRectangle();
            var pretenderRect = pretender.ToRectangle();
            if(sampleRect.X == pretenderRect.X && sampleRect.Y == pretenderRect.Y
                && sampleRect.Width == pretenderRect.Width && sampleRect.Height==pretenderRect.Height)
                return true;
            return false;
        }

        public bool Touches(COA coa)
        {
            if (this.HasIntersectionWith(coa)) return false;
            
            var sample = this.ToRectangle();
            var pretender = coa.ToRectangle();

            return sample.Touches(pretender);

            return false;
        }

        public int TimesItTouches(VibroTable table)
        {
            var sample = this.ToRectangle();
            return sample.Touches(table);
        }



    }
}
