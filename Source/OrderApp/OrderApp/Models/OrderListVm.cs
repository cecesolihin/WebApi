using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderApp.Models
{
    public class OrderListVm
    {
        public Int64 No { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        //public Customer customer { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        //public OrderDetail orderDetail { get; set; }
        //public string OrderId { get; set; }
        //public string ProductId { get; set; }
        public string Qty { get; set; }
        public string SubTotal { get; set; }
        //public OrderHeader orderHeader { get; set; }
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        // public string CustomerId { get; set; }
        public string RequiredDate { get; set; }
        public string ShipName { get; set; }
        public string TotalPrice { get; set; }
        
    }
}