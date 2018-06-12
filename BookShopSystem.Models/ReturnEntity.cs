using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 返回实体
    /// </summary>
    public class ReturnEntity<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public T Data { get; set; }
    }
}
