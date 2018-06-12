using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    public class PageRequestEntity
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    /// <summary>
    /// 评论
    /// </summary>
    public class CommentRequest : PageRequestEntity
    {
        public long ProductId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : PageRequestEntity
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public long CategorId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
    }
}
