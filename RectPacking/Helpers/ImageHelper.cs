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
        private string FolderTag { get; set; }

        public ImageHelper(AbstractPlacement process, string folderTag = null)
        {
            TableBitmap = new Bitmap(process.VibroTable.Width, process.VibroTable.Height);

            TableBitmap = DrawRectangle(TableBitmap, new Rectangle(0, 0, TableBitmap.Width, TableBitmap.Height)
                , Color.FloralWhite);
            this.FolderTag = folderTag;
            Save(folderTag);
        }

        public void UpdateStatus(AbstractPlacement process, IAction bestCOA = null)
        {
            if (bestCOA == null)
            {
                foreach (var coa in process.Placed)
                {
                    TableBitmap = DrawRectangle(TableBitmap, coa.ToRectangle(), RandomColor());
                }
                Save(this.FolderTag, "final");
                return;
            }
            TableBitmap = DrawRectangle(TableBitmap, bestCOA.ToRectangle(), RandomColor());

            if (process is PlacementProcess)
            {
                foreach (var point in (process as PlacementProcess).MainPoints)
                {
                    TableBitmap = DrawPoint(TableBitmap, Color.Red, point);
                }
            }

            Save(this.FolderTag,(++count).ToString());
        }

        private static Bitmap DrawRectangle(Bitmap tableBitmap, Rectangle rectangle, Color color, bool isTable = true)
        {
            for (var x = rectangle.X; x < rectangle.X + rectangle.Width; x++)
                for (var y = rectangle.Y; y < rectangle.Y + rectangle.Height; y++)
                    tableBitmap.SetPixel(x, y, color);

            if (isTable)
                tableBitmap = DrawBorderForRectangle(tableBitmap, rectangle);

            return tableBitmap;
        }

        private static Bitmap DrawBorderForRectangle(Bitmap tableBitmap, Rectangle rectangle)
        {
            for (var x = rectangle.X; x < rectangle.X + rectangle.Width; x++)
            {
                tableBitmap.SetPixel(x, rectangle.Y, Color.Black);
                tableBitmap.SetPixel(x, rectangle.Y + rectangle.Height - 1, Color.Black);
            }
            for (var y = rectangle.Y; y < rectangle.Y + rectangle.Height; y++)
            {
                tableBitmap.SetPixel(rectangle.X, y, Color.Black);
                tableBitmap.SetPixel(rectangle.X+rectangle.Width-1, y, Color.Black);
            }
            return tableBitmap;
        }

        private static Bitmap DrawPoint(Bitmap tableBitmap, Color color, Point point)
        {
            //todo: refactor - interseption with edges
            const int pointSize = 4;

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
            return DrawRectangle(tableBitmap, new Rectangle(xmin, ymin, pointsizeX, pointsizeY), color, false);
        }

        public void Save(string folderTag = null, string tag = null)
        {
            using (var m = new MemoryStream())//freaky stuff to make it work
            {
                TableBitmap.Save(m, ImageFormat.Jpeg);
                var img = Image.FromStream(m);
                var pathTag = string.IsNullOrEmpty(tag) ? "00.jpg" : "0" + tag + ".jpg";
                folderTag = string.IsNullOrEmpty(folderTag) ? "" :  folderTag + "\\";
                var destination = "C:\\test\\" + folderTag + pathTag;
                img.Save(destination, ImageFormat.Jpeg);
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
