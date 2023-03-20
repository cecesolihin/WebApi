using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderApp.Models
{
    public class OrderDetail
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Qty { get; set; }
        public string SubTotal { get; set; }
    }
}