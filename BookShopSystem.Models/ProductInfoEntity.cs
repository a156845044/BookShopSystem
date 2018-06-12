using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class ProductInfoEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string PublisherName { get; set; }

        public DateTime PublishDate { get; set; }

        public string ISBN { get; set; }

        public int WordsCount { get; set; }

        public decimal UnitPrice { get; set; }

        public string ContentDescription { get; set; }

        public string AurhorDescription { get; set; }

        public string EditorComment { get; set; }

        public string TOC { get; set; }

        public long CategoryId { get; set; }

        public int? Quantity { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父分类名称
        /// </summary>
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// 父类编号
        /// </summary>
        public long ParentCategoryId { get; set; }

        /// <summary>
        /// 月销量
        /// </summary>
        public int MothSales { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int Sales { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
    }
}
