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

        public static double GetOverlayPercent(COA current, COA opponent)
        {
            var thisRect = current.ToRectangle();
            var opponentRect = opponent.ToRectangle();
            if (thisRect.Touches(opponentRect))
            {
                var hasSameY = thisRect.Bottom == opponentRect.Top || thisRect.Top == opponentRect.Bottom;
                var hasSameX = thisRect.Left == opponentRect.Right || thisRect.Right == opponentRect.Left;

                var overlayLength = 0;
                if (hasSameX)
                {
                    overlayLength = thisRect.Width - thisRect.Right > opponentRect.Right
                        ? thisRect.Right - opponentRect.Right
                        : 0;
                    overlayLength = overlayLength - thisRect.Left < opponentRect.Left
                        ? opponentRect.Left - thisRect.Left
                        : 0;
                    return overlayLength/thisRect.Width;
                }
                if (hasSameY)
                {
                    overlayLength = thisRect.Height - thisRect.Bottom > opponentRect.Bottom
                        ? thisRect.Bottom - opponentRect.Bottom
                        : 0;
                    overlayLength = overlayLength - thisRect.Top < opponentRect.Top
                        ? opponentRect.Top - thisRect.Top
                        : 0;
                    return overlayLength / thisRect.Height;
                }
            }

            return 0;

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
