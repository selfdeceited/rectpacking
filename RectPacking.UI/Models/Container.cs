using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class Container
    {
        public long Id { get; set; }
       // public bool IsRoom { get; set; }
        //public virtual ICollection<Container> Tables { get; set; }
       // public virtual Container Room { get; set; }
        //public long RoomId { get; set; }


        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}