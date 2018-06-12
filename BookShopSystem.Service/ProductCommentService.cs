using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopSystem.Utilities;
using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 商品评价
    /// </summary>
    public class ProductCommentService
    {
        /// <summary>
        /// 根据商品编号获取评论列表
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="recordCount">总页数</param>
        /// <returns>评论列表</returns>
        public List<CommentInfoEntity> GetCommentList(long productId,int pageIndex, int pageSize, out int recordCount)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT PC.*,BU.Account FROM [dbo].[ProductComment]  PC INNER JOIN [dbo].[BaseUser] BU ON BU.Id=PC.[UserId] ");
            strSQL.AppendFormat(" WHERE PC.[ProductId]={0} and pc.[StateFlag]=1 ", productId);

            string countSQL = strSQL.ToString().PageCount();
            using (var ctx = new BookShopContext())
            {
                recordCount = ctx.Database.SqlQuery<int>(countSQL).FirstOrDefault();
                return ctx.Database.SqlQuery<CommentInfoEntity>(strSQL.ToString().ToString().PageLimit(pageIndex, pageSize, " pc.[CreateTime] desc ")).ToList();
            }
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="list">待添加的列表</param>
        /// <returns></returns>
        public bool Add(List<ProductComment> list)
        {
            bool flag = false;
            using (var ctx = new BookShopContext())
            {
                ctx.ProductComment.AddRange(list);
              flag=  ctx.SaveChanges() > 0;
            }
            return flag;

        }
    }
}
