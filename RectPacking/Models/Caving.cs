using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Extensions;
using RectPacking.Helpers;
using RectPacking.Operations;

namespace RectPacking.Models
{
    public class Caving: COA
    {
        public double Degree { get; set; }
        public int EdgeIndex { get; set; }

        public Caving(COA coa, PlacementProcess placement)
            : base(coa.Product,coa.MainPoint, coa.Corner, false, coa.VibroTable)
            //rotated is false, because when you use created COA, it has already rotated product;
        {
            this.Degree = CalculateDegree(coa, placement);
            this.EdgeIndex = CalculateEdgeIndex(coa, placement);
        }

        private int CalculateEdgeIndex(COA coa, PlacementProcess placement)
        {
            return placement.Placed.Count(coa.Touches) + coa.TimesItTouches(placement.VibroTable);
        }

        private double CalculateDegree(COA coa, PlacementProcess placement)
        {
            var distance = CalculateDistance(coa, placement);
            return distance / Math.Sqrt(coa.Width * coa.Height);
        }

        private int CalculateDistance(COA coa, PlacementProcess placement)
        {
           // todo:refactor!
            var table = placement.VibroTable;

            List<COA> placedCOAs = placement.Placed;

            var leftArea = new Segment(Segment.Direction.Left);
            var coasLeft = FindLocalFor(coa, placedCOAs, ref leftArea);
                           // .Where(c => c.Left.IsBetweenIncl(leftArea));
            var dminLeft = coasLeft.Any()
                ? coasLeft.Min(c => coa.Left - (c.Left + c.Width))
                : coa.Left - 0;
                       

            var rightArea = new Segment(Segment.Direction.Right);
            var coasRight = FindLocalFor(coa, placedCOAs, ref rightArea);
            var dminRight = coasRight.Any()
                ? coasRight.Min(c => c.Left - (coa.Left + coa.Width))
                : table.Width - (coa.Left + coa.Width);

            var upArea = new Segment(Segment.Direction.Up);
            var coasUp = FindLocalFor(coa, placedCOAs, ref upArea);
            var dminUp = coasUp.Any()
                ? coasUp.Min(c => coa.Top - (c.Top + c.Height))
                : coa.Top;

            var downArea = new Segment(Segment.Direction.Up);
            var coasDown = FindLocalFor(coa, placedCOAs, ref downArea);
            var dminDown = coasDown.Any()
                ? coasDown.Min(c => c.Top - (coa.Top + coa.Height)):
                table.Height - (coa.Top + coa.Height);

            var dx = Math.Max(dminLeft, dminRight);
            var dy = Math.Max(dminDown, dminUp);
            var dmin = Math.Min(dx, dy);

            return dmin;
        }

        private IEnumerable<COA> FindLocalFor(COA coa, List<COA> placed, ref Segment segment)
        {
            //segment.Define(coa, placed);


            //do not forget about the table!
            switch (segment.ToGo)
            {
                   case Segment.Direction.Left:
                    return placed.Where(a =>
                    a.Left + a.Width < coa.Left && InHorisontalSegment(a, coa));
                    break;
                   case Segment.Direction.Right:
                    return placed.Where(a =>
                    a.Left > coa.Left + coa.Height && InHorisontalSegment(a, coa));
                    break;
                   case Segment.Direction.Up:
                    return placed.Where(a =>
                    a.Top + a.Height < coa.Top && InVerticalSegment(a, coa));
                    break;
                   case Segment.Direction.Down:
                    return placed.Where(a =>
                    a.Top > coa.Top + coa.Height && InVerticalSegment(a, coa));
                    break;
                   default:
                    throw new Exception("something went wrong");
                    break;
            }

            return null;
        }
        
        private bool InHorisontalSegment(COA a, COA coa)
        {
            return !((a.Top < coa.Top && a.Top + a.Height < coa.Top + coa.Height)
                  || (a.Top > coa.Top && a.Top + a.Height > coa.Top + coa.Height));
        }
        private bool InVerticalSegment(COA a, COA coa)
        {
            return !((a.Left < coa.Left && a.Left + a.Width < coa.Left + coa.Width)
                  || (a.Left > coa.Left && a.Left + a.Width > coa.Left + coa.Width));
        }
    }
}
