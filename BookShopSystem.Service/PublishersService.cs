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
    /// 出版社
    /// </summary>
    public class PublishersService
    {
        /// <summary>
        /// 获取出版社列表
        /// </summary>
        /// <returns>出版社列表</returns>
        public List<Publishers> GetPublisherList()
        {
            using (var ctx = new BookShopContext())
            {
                var sql = from p in ctx.Publishers select p;
                return sql.ToList();
            }
        }
    }
}
