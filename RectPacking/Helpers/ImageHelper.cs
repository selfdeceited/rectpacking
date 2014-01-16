using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;
using Point = RectPacking.Models.Point;

namespace RectPacking.Helpers
{
    public class ImageHelper
    {
        private static int count;
        private Bitmap TableBitmap { get; set; }

        public ImageHelper(PlacementProcess process)
        {
            TableBitmap = new Bitmap(process.VibroTable.Width, process.VibroTable.Height);

            TableBitmap = DrawRectangle(TableBitmap, new Rectangle(0, 0, TableBitmap.Width, TableBitmap.Height)
                , Color.FloralWhite);

            Save();
        }

        public void UpdateStatus(PlacementProcess process, COA bestCOA = null)
        {
            if (bestCOA == null)
            {
                foreach (var coa in process.PlacedCOAs)
                {
                    TableBitmap = DrawRectangle(TableBitmap, coa.ToRectangle(), RandomColor());
                }
                Save("final");
                return;
            }
            TableBitmap = DrawRectangle(TableBitmap, bestCOA.ToRectangle(), RandomColor());
            foreach (var point in process.MainPoints)
            {
                TableBitmap = DrawPoint(TableBitmap, Color.Red, point);
            }
            Save((++count).ToString());
        }

        private static Bitmap DrawRectangle(Bitmap tableBitmap, Rectangle rectangle, Color color)
        {
            for (var x = rectangle.X; x < rectangle.X + rectangle.Width; x++)
                for (var y = rectangle.Y; y < rectangle.Y + rectangle.Height; y++)
                    tableBitmap.SetPixel(x, y, color);

            return tableBitmap;
        }
        private static Bitmap DrawPoint(Bitmap tableBitmap, Color color, Point point)
        {
            //todo: refactor - interseption with edges
            const int pointSize = 10;

            var pointsizeX = pointSize;
            var pointsizeY = pointSize;
            bool isHalvedX = false;
            bool isHalvedY = false;

            var xmin = point.X - pointSize / 2;
            var ymin = point.Y - pointSize / 2;
            
            if (xmin <= 0)
            {
                pointsizeX /= 2;
                xmin = 0;
                isHalvedX = true;
            }
            if (ymin <= 0)
            {
                pointsizeY /= 2;
                ymin = 0;
                isHalvedY = true;
            }
            if (xmin + pointsizeX > tableBitmap.Width)
            {
                if (!isHalvedX) pointsizeX /= 2;
                xmin = tableBitmap.Width - pointsizeX;
            }
            if (ymin + pointsizeY > tableBitmap.Height)
            {
                if (!isHalvedY) pointsizeY /= 2;
                ymin = tableBitmap.Height - pointsizeY;
            }
            return DrawRectangle(tableBitmap, new Rectangle(xmin, ymin, pointsizeX, pointsizeY), color);
        }

        public void Save(string tag = null)
        {
            using (var m = new MemoryStream())//freaky stuff to make it work
            {
                TableBitmap.Save(m, ImageFormat.Jpeg);
                var img = Image.FromStream(m);
                var pathTag = string.IsNullOrEmpty(tag) ? "00.jpg" : "0" + tag + ".jpg";
                img.Save("C:\\test\\" + pathTag, ImageFormat.Jpeg);
            }
        }

        private static Color RandomColor()
        {
            var appropriateColors = new HashSet<Color>
            {
                Color.Chartreuse,
                Color.LightBlue,
                Color.DarkSeaGreen,
                Color.MediumAquamarine,
                Color.CornflowerBlue,
                Color.LightSkyBlue,
                Color.LightGreen,
                Color.Plum
            };
            var index = new Random(DateTime.Now.Millisecond).Next(appropriateColors.Count);
            return appropriateColors.ElementAt(index);
        }
    }
}
