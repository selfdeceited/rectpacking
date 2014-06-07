using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class Order
    {
        public long Id { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public virtual Client Client { get; set; }
        public long ClientId { get; set; }

        public int Rating { get; set; }

        public virtual ICollection<ProductSet> ProductSets { get; set; }

    }
}