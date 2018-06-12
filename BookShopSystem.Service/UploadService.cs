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
    /// 上传
    /// </summary>
    public class UploadService
    {
        /// <summary>
        /// 获取上传文件列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sourceId">来源编号</param>
        /// <returns>文件列表</returns>
        public List<FileUpload> GetFileList(int type, string sourceId)
        {
            using (var ctx = new BookShopContext())
            {
                var sql = from u in ctx.FileUpload where u.SourceType == type && u.SourceId == sourceId orderby u.Id ascending select u;
                return sql.ToList();
            }
        }
    }
}
