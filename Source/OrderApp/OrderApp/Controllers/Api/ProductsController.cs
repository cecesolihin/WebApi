using OrderApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace OrderApp.Controllers.Api
{
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAllProduct()//https://localhost:44355/api/products/
        {
            var sql = @"SELECT [ProductId]
                         ,ProductName
                         ,Cast(Price as Varchar) Price
                     FROM[dbo].[Product]";
            var listData = db.Database.SqlQuery<Product>(sql).ToList();
            return Ok(listData);
        }
        [System.Web.Http.HttpGet]
        public IList<Product> ProductAll(string productId, string productName) //https://localhost:44355/api/products?ProductId=PR0001&ProductName=
        {
            var sql = "exec [dbo].[Sp_Product_Search] @ProductId, @ProductName";
            var listData = db.Database.SqlQuery<Product>(sql, new SqlParameter("@ProductId", string.IsNullOrEmpty(productId) ? "" : productId)
                                                            , new SqlParameter("@ProductName", string.IsNullOrEmpty(productName) ? "" : productName)).ToList();
            
            return listData;
        }
        [System.Web.Http.HttpGet]
        public IList<Product> ProductById(string productid)//https://localhost:44355/api/products?ProductId=PR0005
        {
            var sql = "exec [dbo].[Sp_Product_ById] @ProductId";
            var listData = db.Database.SqlQuery<Product>(sql, new SqlParameter("@ProductId", string.IsNullOrEmpty(productid) ? "" : productid)).ToList();
            return listData;
        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddProduct(string productName, string price)//https://localhost:44355/api/products?ProductName=tes2&Price=23000
        {
            string productId= string.Empty;
            var sql = "exec [dbo].[Sp_Product_Insert] @ProductId, @ProductName,@Price";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@ProductId", productId)
                                                        , new SqlParameter("@ProductName", string.IsNullOrEmpty(productName) ? "" : productName)
                                                        , new SqlParameter("@Price", string.IsNullOrEmpty(price) ? "" : price));
            return Ok();
        }
        [System.Web.Http.HttpPut]
        public IHttpActionResult UpdateProduct(string productId,string productName, string price)
        {
            var sql = "exec [dbo].[Sp_Product_Update] @ProductId, @ProductName,@Price";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@ProductId", productId)
                                                        , new SqlParameter("@ProductName", string.IsNullOrEmpty(productName) ? "" : productName)
                                                        , new SqlParameter("@Price", string.IsNullOrEmpty(price) ? "" : price));
            return Ok();
        }
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteProduct(string productId)//https://localhost:44355/api/products?productId=PR0009
        {
            var sql = "exec [dbo].[Sp_Product_Delete] @ProductId";
            var exec = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@ProductId", productId));
            return Ok();
        }
    }
}
