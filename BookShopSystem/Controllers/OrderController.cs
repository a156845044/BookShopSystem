using BookShopSystem.Models;
using BookShopSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookShopSystem.Utilities;
using BookShopSystem.Filters;
using BookShopSystem.DataAccess.Entity;

namespace BookShopSystem.Controllers
{
    public class OrderController : FilterController
    {
        // GET: Order
        [UserAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Order
        public ActionResult Pay()
        {
            return View();
        }

        //
        public ActionResult PaySuccess()
        {
            return View();
        }

        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingCart()
        {
            return View();
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        [UserAuthorizeAttribute]
        public ActionResult Comment()
        {
            return View();
        }

        /// <summary>
        /// 获取我的购物车数量
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetShopingcartNum()
        {
            int num = 0;
            var user = LoginUser;
            if (user != null)
            {
                num = new OrderService().GetShopingCartNum(user.UserId);
            }
            return JsonCResult(num);
        }

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <param name="num">数量</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ActionResult AddShopingCart(long productId, int num)
        {
            bool flag = false;
            var user = LoginUser;
            if (user != null)
            {
                var cart = new OrderService().AddShopingCart(user.UserId, productId, num);
                if (cart != null)
                {
                    flag = true;
                }
            }
            return JsonCResult(flag);
        }

        /// <summary>
        /// 获取待付款的商品列表
        /// </summary>
        /// <param name="payInfoList">付款商品信息</param>
        /// <returns>商品列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetPayProductList(List<PayInfoEntity> payInfoList)
        {
            string idList = payInfoList.Select(e => e.ProductId).ToArray().ToSQLInChar();
            var list = new BookService().GetPayInfoList(idList);
            foreach (var item in list)
            {
                var payItem = payInfoList.Find(e => e.ProductId == item.Id);
                if (payItem != null)
                {
                    item.BuyNum = payItem.BuyNum;
                }
            }
            return JsonCResult(list);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="list">购买列表</param>
        /// <param name="address">地址</param>
        /// <param name="tel">联系电话</param>
        /// <param name="contacts">联系人</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrder(List<PayProductInfoEntity> list, string address, string tel, string contacts)
        {
            bool flag = false;
            var user = LoginUser;
            if (user != null)
            {
                if (list.Count > 0)
                {
                    flag = new OrderService().AddOrder(user.UserId, address, tel, contacts, list);
                }
            }
            return JsonCResult(flag);
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMyOrderList()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            var user = LoginUser;
            if (user != null)
            {
                list = new OrderService().GetMyOrderList(user.UserId);
            }
            return JsonCResult(list);
        }

        /// <summary>
        /// 获取待评价的商品列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetInCommendList(long orderId)
        {
            var list = new OrderService().GetMyOrderProductList(orderId);
            return JsonCResult(list);
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="list">评论列表</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ActionResult AddComment(long orderId, List<OrderDetailEntity> list)
        {
            bool flag = false;
            var user = this.LoginUser;
            if (user == null)
            {
                return JsonCResult(flag);
            }
            OrderService service = new OrderService();
            var model = service.GetModel(orderId);
            if (model == null)
            {
                return JsonCResult(flag);
            }
            List<ProductComment> cList = new List<ProductComment>();
            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.CommentContent))
                {
                    item.CommentContent = "该用户很懒，没有留下任何内容。";
                }
                cList.Add(new ProductComment { StateFlag = 1, Contents = item.CommentContent, CreateTime = DateTime.Now, OrderNum = model.OrderNo, ProductId = item.ProductId, UserId = user.UserId });
            }
            if (cList.Count > 0)
            {
                flag = new ProductCommentService().Add(cList);
            }
            return JsonCResult(flag);
        }
    }
}

