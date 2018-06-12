using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Models
{
    /// <summary>
    /// 分页返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageReturnEntity<T>
    {
        public int PageCount { get; set; }

        public T Data { get; set; }
    }
}
