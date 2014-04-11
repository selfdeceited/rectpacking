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

        public Room Room { get; set; }
        public ImageHelper Image { get; set; }
        public List<IAction> Left { get; set; }
        public List<IAction> OnTable { get; set; }
        public List<IAction> Done { get; set; }
        public VibroTable VibroTable { get; set; }
        public List<Product> ProductList { get; set; }
        public TimeLine TimeLine { get; set; }
        public int Iteration { get; set; }

        public AbstractPlacement(VibroTable vibroTable, List<Product> productList, DateTime startDate)
            : this(vibroTable, productList){
            TimeLine.StartDate = startDate;
        }
        public AbstractPlacement(VibroTable vibroTable, List<Product> productList)
        {
            if (vibroTable is Room)
            {
                this.Room = vibroTable as Room;
            }
            else
            {
                this.VibroTable = vibroTable;
            }
            this.ProductList = productList;
            this.Left = new List<IAction>();
            this.OnTable = new List<IAction>();
            this.Done = new List<IAction>();
            this.TimeLine = new TimeLine();
            TimeLine.StartDate = DateTime.Now;
        }
        public abstract void Proceed(Strategy manager, bool debug = false, string folderTag = null);
        public abstract void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false);

        public virtual void ProceedRoom(Strategy manager, bool debug = false, string folderTag = null)
        {
            var room = this.Room;
            if (room == null) return;

            this.VibroTable = room;
            this.Image = new ImageHelper(this);


            for (int i = 0; i < room.VibroTables.Count; i++)
            {
                this.VibroTable = room.VibroTables[i];
                this.Image.DrawTable(this.VibroTable);
                ProceedFrom(manager, this.OnTable, this.Image, this.Iteration, debug);
            }
            room.CreateBounds();
            //todo: Create New Bounds in the place and shape for tables and place in filters is this.vibrotable is room;
            this.VibroTable = room;
            ProceedFrom(manager, this.OnTable, this.Image, this.Iteration, debug);
        }
    }

}
