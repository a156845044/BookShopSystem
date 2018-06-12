using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using BookShopSystem.Service;
using BookShopSystem.Utilities;
using BookShopSystem.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Controllers
{
    public class HomeController : FilterController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            SessionData.Member.LoginedUser = null;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 获取商城首页分类列表
        /// </summary>
        /// <returns>分类列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetIndexCategoryList()
        {
            var list = new CategoryService().GetBookCategoryList();
            return JsonCResult(list);
        }

        /// <summary>
        /// 获取好书推荐
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetBookRecommendList()
        {
            BookService service = new BookService();
            List<BookInfoEntity> list = null;
            bool flag = false;
            if (IsLogined)
            {
                var user = LoginUser;
                if (user.ProfessionId > 0)
                {
                    list = service.GetTopNList(user.ProfessionId);
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                list = service.GetTopNList();
            }
            return JsonCResult(list);
        }

        /// <summary>
        /// 根据分类获取图书列表
        /// </summary>
        /// <param name="categoryId">分类编号</param>
        /// <returns>图书列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetBookBlockList(long categoryId)
        {
            var list = new BookService().GetTopNListByCategory(categoryId, 6);
            return JsonCResult(list);
        }

        /// <summary>
        /// 注册帐号
        /// </summary>
        /// <param name="model">待注册的实体</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterMember(RegisterEntity model)
        {
            BaseUserService userService = new BaseUserService();
            ReturnEntity<bool> data = new ReturnEntity<bool>
            {
                Status = false
            };
            if (string.IsNullOrWhiteSpace(model.Account))
            {
                data.Msg = "帐号未填写";
            }
            else if (string.IsNullOrWhiteSpace(model.Pwd))
            {
                data.Msg = "密码未填写";
            }
            else if (userService.GetAccoutExistCheck(model.Account))
            {
                data.Msg = "该帐号已存在！";
            }
            else
            {
                BaseUser user = new BaseUser
                {
                    Account = model.Account,
                    Pwd = EncryptionHelper.Md5(model.Pwd),
                    RoleType = 0,
                    ProfessionId = 0
                };
                bool flag = userService.Add(user);
                if (flag)
                {
                    //记录session
                    MemberEntity member = new MemberEntity
                    {
                        UserId = user.Id,
                        Account = user.Account,
                        ProfessionId = user.ProfessionId.GetValueOrDefault()
                    };
                    SessionData.Member.LoginedUser = member;
                    data.Msg = "注册成功！";
                    data.Status = true;
                }
            }

            return JsonCResult(data);
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

            var user = new BaseUserService().GetUserInfoByAccount(account, 0);
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
            MemberEntity member = new MemberEntity
            {
                UserId = user.Id,
                Account = user.Account,
                ProfessionId = user.ProfessionId.GetValueOrDefault()
            };
            SessionData.Member.LoginedUser = member;
            data.Status = true;
            return JsonCResult(data);
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLoginInfo()
        {
            return JsonCResult(LoginUser);
        }

        /// <summary>
        /// 获取搜索列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSearchList(SearchRequest req)
        {
            int recordCount = 0;
            var list = new BookService().GetBookList(req.CategorId, req.Key, req.PageIndex, req.PageSize, out recordCount);
            int count = SysHelper.GetPageCount(recordCount, req.PageSize);
            var data = new PageReturnEntity<List<BookInfoEntity>> { PageCount = count, Data = list };
            return JsonCResult(data);
        }
    }
}
