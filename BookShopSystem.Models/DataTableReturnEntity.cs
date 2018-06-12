using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableReturnEntity<T>
    {
        /// <summary>
        /// 重绘次数
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> data { get; set; }
    }
}
