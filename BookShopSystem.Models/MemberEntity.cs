using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 会员
    /// </summary>
    public class MemberEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public long ProfessionId { get; set; }
    }
}
