using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    public class LoginEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public string Profession { get; set; }
    }
}
