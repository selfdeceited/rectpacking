using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class COA
    {
        public long Id { get; set; }

        public virtual Product Product { get; set; }
        public long ProductId { get; set; }

        public virtual Frame Frame { get; set; }
        public long FrameId { get; set; }

        public virtual Container Container { get; set; }
        public long ContainerId { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }

        public bool IsRotated { get; set; }


    }
}