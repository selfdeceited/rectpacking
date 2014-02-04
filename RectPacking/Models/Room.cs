using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public class Room: VibroTable
    {
        public List<VibroTable> VibroTables { get; set; }
        public List<Rectangle> PlacedTables { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Room(int Width, int Height, params VibroTable[] tables) : base(Width, Height)
        {
            this.VibroTables = new List<VibroTable>();
            this.PlacedTables = new List<Rectangle>();
            this.VibroTables.AddRange(tables);
            
        }
        public Room(int Width, int Height) : base(Width, Height)
        {
            this.VibroTables = new List<VibroTable>();
            this.PlacedTables = new List<Rectangle>();
        }

        public void PlaceTable(VibroTable table, int left, int top)
        {
            table.Left = left;
            table.Top = top;
            this.VibroTables.Add(table);
        }

        public void AutoPlace()
        {
            
        }

        public void CreateBounds()
        {
            foreach (var vibroTable in VibroTables)
            {
                var rect = vibroTable.ToRectangle();
                PlacedTables.Add(rect);
            }
        }
    }
}
