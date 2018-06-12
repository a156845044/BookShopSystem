using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    public class BookPayInfoEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public decimal UnitPrice { get; set; }

        public int? Quantity { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyNum { get; set; }
    }
}
