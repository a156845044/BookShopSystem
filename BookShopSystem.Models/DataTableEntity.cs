using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary> 
    /// 分页
    /// </summary>
    public class DataTableEntity
    {
        /// <summary> 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                Length = Length == 0 ? 20 : Length;
                return (Start / Length) + 1;
            }
        }

        /// <summary> 每页显示的记录数
        /// </summary>
        public int PageSize
        {
            get { return Length; }
            set { Length = value; }
        }

        /// <summary> 
        /// 重绘次数
        /// </summary>
        public int Draw { get; set; }

        /// <summary> 
        /// 每页显示的数量
        /// </summary>
        public int Length { get; set; }

        /// <summary> 
        /// 数据源开始下标
        /// </summary>
        public int Start { get; set; }

        /// <summary> 
        /// 数据列对象
        /// </summary>
        public List<Columns> Columns { get; set; }
    }

    public class Columns
    {
        /// <summary> 
        /// 对应数据库字段名
        /// </summary>
        public string Data { get; set; }
    }
}
