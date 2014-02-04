using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Extensions
{
    public static class RectangleExtension
    {
        public static bool Touches(this Rectangle sample, Rectangle pretender)
        {
            var hasSameY = sample.Bottom == pretender.Top || sample.Top == pretender.Bottom;
            var hasSameX = sample.Left == pretender.Right || sample.Right == pretender.Left;

            //exterminate points-only connection
            if (hasSameX && hasSameY) return false;

            if (hasSameY && HasPointBetween(pretender.Left, pretender.Right, sample.Left, sample.Right)) return true;
            if (hasSameX && HasPointBetween(pretender.Top, pretender.Bottom, sample.Top, sample.Bottom)) return true;

            return false;
        }

        public static int Touches(this Rectangle sample, VibroTable table)
        {
            var pretender = new Rectangle(0, 0, table.Width, table.Height);
            var top = sample.Top == pretender.Top;
            var bottom = sample.Bottom == pretender.Bottom;
            var left = sample.Left == pretender.Left;
            var right = sample.Right == pretender.Right;
            var result = (top ? 1 : 0) + (bottom ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);
            return result;
        }

        public static bool HasPointBetween(int pretendermin, int pretendermax, int samplemin, int samplemax)
        {
            var result = pretendermin.IsBetweenIncl(samplemin, samplemax)
                   || pretendermax.IsBetweenIncl(samplemin, samplemax);

            result = result || samplemin.IsBetweenIncl(pretendermin, pretendermax)
                   || samplemax.IsBetweenIncl(pretendermin, pretendermax);

            return result;
        }
    }
}
