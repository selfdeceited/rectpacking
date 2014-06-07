using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class User
    {
        public long Id { get; set; }

        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }

        public string UserName { get; set; }
        public string SaltedPassword { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } 
        public bool Enabled { get; set; }
    }
}