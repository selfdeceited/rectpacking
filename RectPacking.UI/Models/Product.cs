using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class Product
    {
        public long Id { get; set; }

        public virtual Image Image { get; set; }
        public long ImageId { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public string StandardName { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan FreezeTime { get; set; }

        public virtual ConcreteType ConcreteType { get; set; }
        public long ConcreteTypeId { get; set; }

        public virtual ICollection<ProductSet> ProductSets { get; set; }

        public bool IsReady { get; set; }
    }
}