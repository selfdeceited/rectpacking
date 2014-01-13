using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class Product
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public long Area { get; set; }
        public bool IsValid { get; set; }
        public Product(string Name, int Width, int Height)
        {
            this.Name = Name;
            this.Width = Width;
            this.Height = Height;
            this.Area = Width * Height;
            this.IsValid = Width > 0 && Height > 0;
        }

        public bool CanContainIn(VibroTable vibroTable)
        {
            return CanContainInArea(vibroTable.Width, vibroTable.Height);
        }

        public bool CanContainInArea(int Width, int Height)
        {
            if (this.Width == 0 || this.Height == 0) return false;
            if (this.IsBiggerThan(Width, Height))
            {
                Rotate();
                if (this.IsBiggerThan(Width, Height)) return false;
                Rotate();//rotate back to initial condition
            }
            return true;
        }

        public void Rotate()
        {
            var temp = this.Width;
            this.Width = this.Height;
            this.Height = temp;
        }

        public bool IsBiggerThan(int Width, int Height)
        {
            return this.Width > Width || this.Height > Height;
        }
    }
}
