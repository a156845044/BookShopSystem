using BookShopSystem.Models;
using BookShopSystem.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Controllers
{
    public class FilterController : Controller
    {
        public FilterController()
        {
            ViewBag.IsLogined = 0;
            if (SessionData.Member.IsLogined)
            {
                ViewBag.UserName = LoginUser.Account;
                ViewBag.IsLogined = 1;
            }

        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public MemberEntity LoginUser
        {
            get
            {
                return SessionData.Member.LoginedUser;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogined
        {
            get { return SessionData.Member.IsLogined; }
                 
        }


        /// 返回Json格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ActionResult JsonCResult<T>(T data)
        {
            return new JsonCResult<T>(data);
        }

    }
}