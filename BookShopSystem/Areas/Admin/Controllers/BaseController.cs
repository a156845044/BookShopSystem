using BookShopSystem.Filters;
using BookShopSystem.Models;
using BookShopSystem.Utilities;
using BookShopSystem.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Controllers
{
    /// <summary>
    /// 基础
    /// </summary>

    [AdminAuthorizeAttribute]
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (SessionData.Admin.IsLogined)
            {
                ViewBag.UserName = LoginUser.Account;
            }

        }

        /// <summary>
        /// 返回Json格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ActionResult JsonCResult<T>(T data)
        {
            return new JsonCResult<T>(data);
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public LoginEntity LoginUser
        {
            get
            {
                return SessionData.Admin.LoginedUser;
            }
        }

    }
}