using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderApp.Models
{
    public class OrderVm
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        // public string CustomerId { get; set; }
        public string RequiredDate { get; set; }
        public string ShipName { get; set; }
        public string Action { get; set; }
        public string TotalPrice { get; set; }

        public IList<OrderDetail> orderList { get; set; }
    }
}