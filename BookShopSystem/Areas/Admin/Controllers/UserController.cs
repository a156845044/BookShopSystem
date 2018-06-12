using BookShopSystem.Controllers;
using BookShopSystem.Models;
using BookShopSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取人员管理列表
        /// </summary>
        /// <param name="pModel">dataTable请求实体</param>
        /// <param name="key">关键字</param>
        /// <returns>人员管理列表</returns>
        [HttpPost]
        public ActionResult GetUserList(DataTableEntity pModel, string key)
        {
            int recordCount = 0;
            var list = new BaseUserService().GetUserMgrList(key, pModel.PageIndex, pModel.PageSize, out recordCount);
            var data = new DataTableReturnEntity<UserMgrEntity>
            {
                data = list,
                draw = pModel.Draw,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            };
            return JsonCResult(data);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">待删除的主键</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(long id)
        {
            bool flag = new BaseUserService().Delete(id);
            return JsonCResult(flag);
        }
    }
}