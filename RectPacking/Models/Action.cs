using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class Action: IAction
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public double Distance { get; set; }
        public Product Product { get; set; }
        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Width, Height);
        }

        public bool CanContainIn(VibroTable table, int XOffset, int YOffset)
        {
            return XOffset + Width <= table.Width && YOffset + Height <= table.Height;
        }
    }
}
