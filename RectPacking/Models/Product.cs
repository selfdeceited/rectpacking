using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RectPacking.Models
{
    public class Product
    {
        public int Identifier { get; set; }
        private static int identityCount;

        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public long Area { get; set; }
        public bool IsValid { get; set; }
        public TimeSpan FreezeTime { get; set; }

        public Product(string Name, int Width, int Height, bool increaseIdentity = true)
        {
            this.FreezeTime = new TimeSpan(0, 30, 0);
            this.Name = Name;
            this.Width = Width;
            this.Height = Height;
            this.Area = Width * Height;
            this.IsValid = Width > 0 && Height > 0;
            this.Identifier = increaseIdentity ? ++identityCount : identityCount;
        }
        public Product(int MinutesToFreeze, string Name, int Width, int Height, bool increaseIdentity = true)
        {
            this.Name = Name;
            this.FreezeTime = new TimeSpan(0, MinutesToFreeze, 0);
            this.Width = Width;
            this.Height = Height;
            this.Area = Width * Height;
            this.IsValid = Width > 0 && Height > 0;
            this.Identifier = increaseIdentity ? ++identityCount : identityCount;
        }
        public Product(int Width, int Height, bool increaseIdentity = true)
        {
            this.FreezeTime = new TimeSpan(0, 30, 0);
            //todo: REFACTOR (WITHOUT USING DUBLICATES)
            this.Name = "";
            this.Width = Width;
            this.Height = Height;
            this.Area = Width * Height;
            this.IsValid = Width > 0 && Height > 0;
            this.Identifier = increaseIdentity ? ++identityCount : identityCount;
        }

        private Product()
        {
            //is needed to write new Product {blah} stuff in Dublicate
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

        public Product Dublicate()
        {
            return new Product
            {
                Width = this.Width,
                Height = this.Height,
                Area = this.Width * this.Height,
                Identifier = this.Identifier,
                IsValid = this.IsValid,
                Name = this.Name,
                FreezeTime = this.FreezeTime
                
            };
        }

        public static List<Product> RemoveFromListWithId(int id, List<Product> list)
        {
            var productToDelete = list.Find(p => p.Identifier == id);
            list.Remove(productToDelete);
            return list;
        }
    }
}
