using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 评价
    /// </summary>
    public class CommentInfoEntity
    {
        public long Id { get; set; }

        public int StateFlag { get; set; }

        public string Account { get; set; }

        public DateTime CreateTime { get; set; }

        public string Contents { get; set; }

    }
}
