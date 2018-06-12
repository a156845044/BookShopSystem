using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 分类
    /// </summary>
    public class CategoryEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子类
        /// </summary>
        public List<ClildCategoryEntity> ChildList { get; set; }
    }

    /// <summary>
    /// 子类
    /// </summary>
    public class ClildCategoryEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
