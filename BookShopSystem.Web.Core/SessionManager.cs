using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BookShopSystem.Web.Core
{
    /// <summary>
    /// Session管理
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// 获取Session中对象
        /// </summary>
        public static T GetSession<T>(string key)
        {
            object obj = HttpContext.Current.Session[key];
            if (obj == null)
                return default(T);
            else
                return (T)obj;
        }

        /// <summary>
        /// 设置Session中的对象
        /// </summary>
        public static void SetSession<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        /// <summary>
        /// 清除Session对象
        /// </summary>
        public static void ClearSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        /// 清除所有Session对象
        /// </summary>
        public static void ClearAllSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
