using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Strategies;

namespace RectPacking.Operations
{
    public abstract class AbstractPlacement
    {
        public ImageHelper Image { get; set; }
        public List<IAction> Left { get; set; }
        public List<IAction> Placed { get; set; }
        public VibroTable VibroTable { get; set; }
        public List<Product> ProductList { get; set; }
        public int Iteration { get; set; }

        public AbstractPlacement(VibroTable vibroTable, List<Product> productList)
        {
            this.VibroTable = vibroTable;
            this.ProductList = productList;
            this.Left = new List<IAction>();
            this.Placed = new List<IAction>();
            
        }

        public abstract void Proceed(Strategy manager, bool debug = false, string folderTag = null);
        public abstract void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false);
    }

}
