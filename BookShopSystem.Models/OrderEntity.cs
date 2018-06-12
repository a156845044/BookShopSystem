using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OrderEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 评论个数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public List<OrderDetailEntity> ProudctList { get; set; }
    }

    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetailEntity
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 订单主键编号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public long ProductId { get; set; }
    }

}
