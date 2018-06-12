using BookShopSystem.Controllers;
using BookShopSystem.Models;
using BookShopSystem.Service;
using BookShopSystem.Utilities;
using BookShopSystem.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookShopSystem.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录页 GET: Admin/Login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="pwd">密码</param>
        /// <returns>登录验证</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitLogin(string account, string pwd)
        {
            ReturnEntity<string> data = new ReturnEntity<string>
            {
                Status = false
            };
            if (string.IsNullOrWhiteSpace(pwd))
            {
                data.Msg = "未输入密码！";
                return JsonCResult(data);
            }
            if (string.IsNullOrWhiteSpace(account))
            {
                data.Msg = "未输入帐号！";
                return JsonCResult(data);
            }

            var user = new BaseUserService().GetUserInfoByAccount(account,1);
            if (user == null)
            {
                data.Msg = "帐号不存在！";
                return JsonCResult(data);
            }
            pwd = EncryptionHelper.Md5(pwd);
            if (user.Pwd.ToLower() != pwd.ToLower())
            {
                data.Msg = "密码错误！";
                return JsonCResult(data);
            }

            //记录session
            SessionData.Admin.LoginedUser = new LoginEntity {
                Account=user.Account,
                UserId=user.Id
            };
            data.Status = true;
            return JsonCResult(data);
        }

        /// <summary>
        /// 退出登录 GET: Admin/Account/LoginOut
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            SessionData.Admin.LoginedUser = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}