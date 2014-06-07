using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class Client
    {
        public long Id { get; set; }

        public string ClientName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int LoyaltyIndex { get; set; }

    }
}