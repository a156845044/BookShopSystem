using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// 获取我的购物车数量
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>购物车数量</returns>
        public int GetShopingCartNum(long userId)
        {
            string sql = string.Format("select count(*) from [dbo].[ShopingCart] where [UserId]={0}", userId);
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="productId">商品编号</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public ShopingCart AddShopingCart(long userId, long productId, int num)
        {
            using (var ctx = new BookShopContext())
            {
                var cart = (from b in ctx.ShopingCart where b.UserId == userId && b.ProductId == productId select b).FirstOrDefault();
                if (cart != null)
                {
                    cart.Quantity = cart.Quantity + num;
                    if (ctx.SaveChanges() > 0)
                    {
                        return cart;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    ShopingCart model = new ShopingCart
                    {
                        ProductId = productId,
                        Quantity = num,
                        UserId = userId,
                    };

                    ctx.ShopingCart.Add(model);
                    if (ctx.SaveChanges() > 0)
                    {
                        return model;
                    }
                    else
                    {
                        return null;
                    }

                }

            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="list">列表</param>
        public bool AddOrder(long userId, string adress, string tel, string contacts, List<PayProductInfoEntity> list)
        {
            bool flag = false;
            string idList = list.Select(e => e.Id).ToArray().ToSQLInChar();
            var productList = new BookService().GetPayInfoList(idList);
            Orders order = new Orders
            {
                Address = adress,
                Contacts = contacts,
                CreateTime = DateTime.Now,
                OrderNo = SysHelper.GetPrimaryKey(),
                StateFlag = FlagMgr.Order.OrderSateFlag.Pay.ToInt(),
                Tel = tel,
                TotalPrice = 0,
                UserId = userId
            };

            decimal total = 0;
            List<OrdersDetail> detailList = new List<OrdersDetail>();
            foreach (var item in productList)
            {
                int number = 0;
                var payItem = list.Find(e => e.Id == item.Id);
                if (payItem != null)
                {
                    number = payItem.BuyNum;
                }
                OrdersDetail model = new OrdersDetail()
                {
                    ProductId = item.Id,
                    Quantity = number,
                    UnitPrice = item.UnitPrice
                };
                total += number * item.UnitPrice;
                detailList.Add(model);
            }
            order.TotalPrice = total;
            using (var ctx = new BookShopContext())
            {
                using (DbContextTransaction transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ctx.Orders.Add(order);
                        if (ctx.SaveChanges() > 0)
                        {
                            flag = true;
                            foreach (var item in detailList)
                            {
                                item.OrderId = order.Id;
                            }
                            ctx.OrdersDetail.AddRange(detailList);
                            ctx.SaveChanges();
                            ctx.Database.ExecuteSqlCommand(string.Format("delete from [dbo].[ShopingCart] where [UserId]={0} and [ProductId] in({1})", userId, idList));
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>订单列表</returns>
        public List<OrderEntity> GetMyOrderList(long userId)
        {
            string sql = string.Format("SELECT *,(SELECT COUNT(*) FROM [dbo].[ProductComment] PC WHERE PC.[OrderNum]=O.[OrderNo] ) AS CommentCount FROM [dbo].[Orders] O WHERE O.[UserId]={0} ORDER by O.[CreateTime] desc ", userId);

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT OD.[OrderId],OD.[Quantity],OD.[UnitPrice],OD.[Id],FU.[FilePath] AS CoverURL,P.Title FROM [dbo].[OrdersDetail] OD ");
            strSQL.Append("INNER JOIN [dbo].[Product] P ON P.Id = OD.[ProductId] ");
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.AppendFormat(" WHERE OD.OrderId In (SELECT Id FROM [dbo].[Orders] WHERE [UserId]={0})",userId);
            List<OrderEntity> orderList = new List<OrderEntity>();
            List<OrderDetailEntity> detailList = new List<OrderDetailEntity>();
            using (var ctx = new BookShopContext())
            {
                orderList = ctx.Database.SqlQuery<OrderEntity>(sql).ToList();
                detailList = ctx.Database.SqlQuery<OrderDetailEntity>(strSQL.ToString()).ToList();
            }
            foreach (var item in orderList)
            {
                item.ProudctList = detailList.FindAll(e => e.OrderId == item.Id).ToList();
            }
            return orderList;
            
        }

        /// <summary>
        /// 获取我的订单商品列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>订单列表</returns>
        public List<OrderDetailEntity> GetMyOrderProductList(long orderId)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT OD.[OrderId],OD.[Quantity],OD.[UnitPrice],OD.[ProductId],OD.[Id],FU.[FilePath] AS CoverURL,P.Title FROM [dbo].[OrdersDetail] OD ");
            strSQL.Append("INNER JOIN [dbo].[Product] P ON P.Id = OD.[ProductId] ");
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.AppendFormat(" WHERE OD.OrderId ={0}", orderId);
            List<OrderDetailEntity> detailList = new List<OrderDetailEntity>();
            using (var ctx = new BookShopContext())
            {
                detailList = ctx.Database.SqlQuery<OrderDetailEntity>(strSQL.ToString()).ToList();
            }
            return detailList;

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Orders GetModel(long Id)
        {
            using (var ctx =new BookShopContext())
            {
                return ctx.Orders.Where(e => e.Id == Id).FirstOrDefault();
            }
        }

    }
}
