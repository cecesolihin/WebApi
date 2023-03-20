using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderApp.Models;
using System.Data.SqlClient;
namespace OrderApp.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [System.Web.Http.HttpGet]
        public IHttpActionResult CustomerAll(string customerId, string customerName,string city)
        {
            var sql = "exec [dbo].[Sp_Customer_Search] @CustomerId, @CustomerName,@City";
            var listData = db.Database.SqlQuery<Customer>(sql, new SqlParameter("@CustomerId", string.IsNullOrEmpty(customerId) ? "" : customerId)
                                                            , new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? "" : customerName)
                                                            , new SqlParameter("@City", string.IsNullOrEmpty(city) ? "" : city)).ToList();
            return Ok(listData);
        }
        [System.Web.Http.HttpGet]
        public IHttpActionResult CustomerById(string customerid)
        {
            var sql = "exec [dbo].[Sp_Customer_ById] @CustomerId";
            var listData = db.Database.SqlQuery<Customer>(sql, new SqlParameter("@CustomerId", string.IsNullOrEmpty(customerid) ? "" : customerid)).ToList();
            return Ok(listData);
        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddCustomer(string customerName, string address, string city)
        {
            string customerId = string.Empty;
            var sql = "exec [dbo].[Sp_Customer_Insert] @CustomerId, @CustomerName,@Address,@City";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@CustomerId", customerId)
                                                        , new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? "" : customerName)
                                                        , new SqlParameter("@Address", string.IsNullOrEmpty(address) ? "" : address)
                                                        , new SqlParameter("@City", city));
            return Ok();
        }
        [System.Web.Http.HttpPut]
        public IHttpActionResult UpdateCustomer(string customerId, string customerName, string address, string city)
        {
            var sql = "exec [dbo].[Sp_Customer_Update] @CustomerId, @CustomerName,@Address,@City";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@CustomerId", customerId)
                                                        , new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? "" : customerName)
                                                        , new SqlParameter("@Address", string.IsNullOrEmpty(address) ? "" : address)
                                                        , new SqlParameter("@City", city));
            return Ok();
        }
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteCustomer(string customerId)
        {
            var sql = "exec [dbo].[Sp_Customer_Delete] @CustomerId";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@CustomerId", customerId));
            return Ok();
        }
    }
}
