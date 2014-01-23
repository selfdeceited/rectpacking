using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Helpers;

namespace RectPacking.Extensions
{
    public static class PointExtension
    {
        public static bool IsBetweenIncl(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }
        public static bool IsBetweenIncl(this int value, Segment segment)
        {
            return value >= segment.Min && value <= segment.Max;
        }
        public static bool IsBetween(this int value, int min, int max)
        {
            return value > min && value < max;
        }
    }
}
