using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class ProductSet
    {
        public long Id { get; set; }

        public virtual Order Order { get; set; }
        public long OrderId { get; set; }

        public virtual Product Product { get; set; }
        public long ProductId { get; set; }

        public int Quantity { get; set; }
    }
}