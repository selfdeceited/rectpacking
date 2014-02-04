using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Operations;

namespace RectPacking.Models
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public bool IsValid { get; set; }

        public Point()
        {
        }

        public Point(int X, int Y, bool IsMain = false)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.IsMain = IsMain;
            this.IsValid = X >= 0 && Y >= 0; 
        }
        public bool IsWithin(VibroTable vibroTable)
        {
            return this.IsWithinIncludedArea(vibroTable.Left, vibroTable.Left + vibroTable.Width, vibroTable.Top, vibroTable.Top+vibroTable.Height);
        }

        public bool IsWithinArea(int xMin, int xMax, int yMin, int yMax)
        {
            return this.X > xMin && this.X < xMax && this.Y > yMin && this.Y < yMax;
        }
        public bool IsWithinIncludedArea(int xMin, int xMax, int yMin, int yMax)
        {
            return this.X >= xMin && this.X <= xMax && this.Y >= yMin && this.Y <= yMax;
        }

        public bool IsOutsideOf(COA a)
        {
            var xMin = a.Points.Min(p => p.X);
            var xMax = a.Points.Max(p => p.X);
            var yMin = a.Points.Min(p => p.Y);
            var yMax = a.Points.Max(p => p.Y);

            return !IsWithinArea(xMin, xMax, yMin, yMax);
        }

        public bool CheckIfHasPointInSameLocation(List<Point> mainPoints)
        {
            return mainPoints
                .Where(point => this != point).
                Aggregate(false, (current, point) => current || point.X == this.X && point.Y == this.Y);
        }
    }
}
