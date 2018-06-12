using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    public class PayInfoEntity
    {

        /// <summary>
        /// 商品编号
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int BuyNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 支付商品信息
    /// </summary>
    public class PayProductInfoEntity
    {

        /// <summary>
        /// 数量
        /// </summary>
        public int BuyNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
    }
}
