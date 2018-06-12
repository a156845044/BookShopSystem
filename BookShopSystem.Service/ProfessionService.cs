using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 职业
    /// </summary>
    public class ProfessionService
    {
        /// <summary>
        /// 获取职业列表
        /// </summary>
        /// <returns></returns>
        public List<Profession> GetList()
        {
            using (var ctx = new BookShopContext())
            {
                var sql = from p in ctx.Profession select p;
                return sql.ToList();
            }
        }
    }
}
