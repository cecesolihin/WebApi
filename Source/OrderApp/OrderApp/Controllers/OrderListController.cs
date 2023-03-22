using OrderApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderApp.Controllers
{
    public class OrderListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: OrderList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(OrderListVm md, int page, int size)
        {

            try
            {
                DoSearch(md, page, size);
            }
            catch (Exception ex)
            {

                return Json("Error : " + ex.Message);
            }
            return PartialView("_GridView");
        }
        public void DoSearch(OrderListVm md, int page, int size)
        {
            int countData = 0;
            if (page == 0)
            {
                countData = 0;
            }
            else
            {
                IList<OrderListVm> data = GetAllData(md);
                countData = data.Count;
            }
            Paging pg = new Paging(countData, page, size);
            ViewData["Paging"] = pg;
            var sql = @"exec Sp_OrderList_Search @OrderNo, @Customer,@Page, @Size";
            IList<OrderListVm> listData = db.Database.SqlQuery<OrderListVm>(sql, new SqlParameter("@OrderNo", String.IsNullOrEmpty(md.OrderId) ? "" : md.OrderId)
                                                                               , new SqlParameter("@Customer", String.IsNullOrEmpty(md.CustomerId) ? "" : md.CustomerId)
                                                                               , new SqlParameter("@Page", page)
                                                                               , new SqlParameter("@Size", size)).ToList();
            ViewData["listdata"] = listData;
          

        }
        public IList<OrderListVm> GetAllData(OrderListVm md)
        {
            var sql = @"exec Sp_OrderList_Count @OrderNo, @Customer ";
            IList<OrderListVm> listData = db.Database.SqlQuery<OrderListVm>(sql, new SqlParameter("@OrderNo", String.IsNullOrEmpty(md.OrderId) ? "" : md.OrderId)
                                                                               , new SqlParameter("@Customer", String.IsNullOrEmpty(md.CustomerId) ? "" : md.CustomerId)
                                                                               ).ToList();
            return listData;
        }
        public JsonResult GetCustomerById(string CustomerId)
        {
            var sql = @"select CustomerId,CustomerName, Address,City from [dbo].[Customer] where CustomerId=@CustomerId";
            var getData = db.Database.SqlQuery<Customer>(sql, new SqlParameter("CustomerId", CustomerId)).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductById(string ProductId)
        {
            var sql = @"select ProductId,ProductName,Cast(Price as varchar) Price from [dbo].[Product] where (nullif(@ProductId,'') is null or ProductId =@ProductId)";
            var getData = db.Database.SqlQuery<Product>(sql, new SqlParameter("ProductId", String.IsNullOrEmpty(ProductId) ? "" : ProductId)).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderHeaderById(string orderid)
        {
            var sql = @"exec Sp_OrderHeaderById @OrderId";
            var getData = db.Database.SqlQuery<OrderHeader>(sql, new SqlParameter("@OrderId", orderid)).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderDetailById(string orderid)
        {
            var sql = @"exec Sp_OrderDetailById @OrderId";
            var getData = db.Database.SqlQuery<OrderListVm>(sql, new SqlParameter("@OrderId", orderid)).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public void GetDataAll()
        {
            var sql2 = @"select ProductId,ProductName,Cast(Price as varchar) Price from [dbo].[Product]";
            IList<Product> listProducts = db.Database.SqlQuery<Product>(sql2).ToList();
            ViewData["listProducts"] = listProducts;

            var sql3 = @"select CustomerId,CustomerName, Address,City from [dbo].[Customer]";
            IList<Customer> listCustomer = db.Database.SqlQuery<Customer>(sql2).ToList();
            ViewData["listCustomer"] = listCustomer;
        }
        public JsonResult GetAllCustomer()
        {
            var sql = @"select DISTINCT CustomerId,CustomerName from [dbo].[Customer]";
            var listCustomer = db.Database.SqlQuery<Customer>(sql).ToList();
            
            //ViewData["listCustomer"] = listCustomer;
            return Json(listCustomer, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetAllProduct()
        //{
        //    var sql = @"select ProductId,ProductName,Cast(Price as varchar) Price from [dbo].[Product]";
        //    var getData = db.Database.SqlQuery<Product>(sql).ToList();
        //    return Json(getData, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult SaveData(OrderVm data)
        {
            string result = string.Empty;
            try
            {
                //SqlParameter outputTblOrderDetail = CreateSqlParameterOutPutTblOrderDetail("TableOfOrderDetail", data.orderList, "dbo.TableOfOrderDetail");

                if (data.Action.ToString() == "ADD")
                {
                    var sql1 = @"SELECT top 1 cast(right(OrderId,4) as int) OrderNo
                                 , OrderId
                                 ,Cast(OrderDate as varchar) OrderDate
                                 ,CustomerId
                                 ,cast(RequiredDate as varchar) RequiredDate
                                 ,ShipName
                                 ,cast(TotalPrice as varchar) TotalPrice
                             FROM [dbo].[OrderHeader] order by cast(right(OrderId,4) as int) desc";
                    IList<OrderHeader> dataOrder = db.Database.SqlQuery<OrderHeader>(sql1).ToList();
                    var vOrderno = dataOrder[0].OrderNo;
                    var sequence = vOrderno + 1;
                    string xOrderNo = data.CustomerId + "0000" + sequence.ToString();

                    for (int i = 0; i < data.orderList.Count; i++)
                    {

                        var sql = "exec Sp_Insert_OrderDetail @OrderId, @CustomerId, @Productid, @Qty, @SubTotal,@OrderDate,@RequiredDate,@ShipName, @TotalPrice ";
                        var execquery = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@OrderId", xOrderNo)
                                                                         , new SqlParameter("@CustomerId", data.CustomerId)
                                                                         , new SqlParameter("@Productid", data.orderList[i].ProductId)
                                                                         , new SqlParameter("@Qty", data.orderList[i].Qty)
                                                                         , new SqlParameter("@SubTotal", data.orderList[i].SubTotal)
                                                                         , new SqlParameter("@OrderDate", data.OrderDate)
                                                                         , new SqlParameter("@RequiredDate", data.RequiredDate)
                                                                         , new SqlParameter("@ShipName", data.ShipName)
                                                                         , new SqlParameter("@TotalPrice", data.TotalPrice));
                    }
                }
                else
                {
                    var sql = "exec Sp_Update_OrderHeader @OrderId,@OrderDate,@RequiredDate,@ShipName, @TotalPrice ";
                    var execquery = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@OrderId", data.OrderId)

                                                                         , new SqlParameter("@OrderDate", data.OrderDate)
                                                                         , new SqlParameter("@RequiredDate", data.RequiredDate)
                                                                         , new SqlParameter("@ShipName", data.ShipName)
                                                                         , new SqlParameter("@TotalPrice", data.TotalPrice));
                }

                result = "success";
            }
            catch (Exception ex)
            {

                result = "Error :" + ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOrder(string OrderId)
        {
            string result = string.Empty;
            try
            {
                var sql = "exec Sp_OrderList_Delete @OrderId";
                var execquery = db.Database.ExecuteSqlCommand(sql, new SqlParameter("@OrderId", OrderId));
                result = "success";
            }
            catch (Exception ex)
            {

                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public string RoleAdmin(User user)
        //{
        //    var sql = "select * from [dbo].[User] where Username=@username";
        //    IList<User> data = db.Database.SqlQuery<User>(sql, new SqlParameter("@username", user.UserName)).ToList();
        //    string isAdmin = data[0].Isadmin.ToString();
        //    return isAdmin;
        //}
    }
}