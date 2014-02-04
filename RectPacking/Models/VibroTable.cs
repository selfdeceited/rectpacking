﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class VibroTable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public long Area { get; set; }
        public bool IsValid { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }

        public VibroTable(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            this.Area = Width * Height;
            this.IsValid = Width > 0 && Height > 0;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Width, Height);
        }
    }
}
