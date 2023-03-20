using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderApp.Models
{
    public class OrderHeader
    {
        public int OrderNo { get; set; }
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string RequiredDate { get; set; }
        public string ShipName { get; set; }
        public string TotalPrice { get; set; }
    }
}