using BookShopSystem.Filters;
using BookShopSystem.Service;
using BookShopSystem.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Controllers
{
    public class PersonController : FilterController
    {
        // GET: Person
        [UserAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthorizeAttribute]
        public ActionResult Information()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="professionId"></param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthorizeAttribute]
        public ActionResult UpdatePersonInfo(long professionId)
        {
            bool flag = false;
            var user = LoginUser;
            if (user != null)
            {
                user.ProfessionId = professionId;
                if (new BaseUserService().UpdateProfession(user.UserId, user.ProfessionId))
                {
                    flag = true;
                    SessionData.Member.LoginedUser = user;
                }
            }
            return JsonCResult(flag);
        }
    }
}