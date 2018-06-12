using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Utilities
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 获取分页可执行SQL(注意，本方法字符串中不能包含order by )
        /// </summary>
        /// <param name="str">当前字符串</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="orderby">排序</param>
        /// <returns>可执行的分页SQL</returns>
        public static string PageLimit(this string str, int pageIndex, int pageSize, string orderby = "(SELECT 1)")
        {
            int startIndex = 0;
            int endIndex = 0;
            startIndex = (pageIndex - 1) * pageSize + 1;
            endIndex = pageSize * pageIndex;
            //return String.Format("SELECT TOP {3} * FROM (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) RN,PT1.* FROM({0})PT1)PT2 WHERE PT2.RN BETWEEN {1} AND {2}", str, startIndex, endIndex, pageSize);
            //此方法有待优化
            if (!string.IsNullOrWhiteSpace(str))
            {
                //去除第一个Select
                char[] chars = new char[] { 's', 'S', 'e', 'E', 'l', 'L', 'e', 'E', 'c', 'C', 't', 'T' };
                return String.Format("SELECT TOP {3} * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {4} ) RN,{0} )PT2 WHERE PT2.RN BETWEEN {1} AND {2}", str.Trim().TrimStart(chars), startIndex, endIndex, pageSize, orderby);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 获取总记录条数可执行SQL
        /// </summary>
        /// <param name="str">当前字符串</param>
        /// <returns>总记录条数可执行SQL</returns>
        public static string PageCount(this string str)
        {
            int i = str.ToLower().IndexOf("from");
            string sql = str;
            if (i > 0)
            {
                sql = sql.Substring(i, sql.Length - i - 1);
                sql = String.Format("SELECT COUNT(1) {0}", sql);
            }
            else
            {
                sql = String.Format("SELECT COUNT(1) FROM({0})PT", sql);
            }
            return sql;
        }

        /// <summary>
        /// 去重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source">当前</param>
        /// <param name="keySelector">lambda表达式</param>
        /// <returns></returns>
        public static IEnumerable<T> ToDistinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }

        /// <summary>
        /// 转换为SQL IN 查询形式的'1','2','4'
        /// </summary>
        /// <param name="array">当前数组</param>
        /// <returns>字符串</returns>
        public static string ToSQLInChar(this object[] array)
        {
            if (array.Length <= 0)
            {
                return "''";
            }
            string str = string.Join(",", array);
            return string.Format("'{0}'", str.Replace(",", "','"));
        }

        /// <summary>
        /// 转换为SQL IN 查询形式的'1','2','4'
        /// </summary>
        /// <param name="array">当前数组</param>
        /// <returns>字符串</returns>
        public static string ToSQLInChar(this long[] array)
        {
            if (array.Length <= 0)
            {
                return "''";
            }
            string str = string.Join(",", array);
            return string.Format("'{0}'", str.Replace(",", "','"));
        }

        /// <summary>
        /// 返回新的拷贝数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">当前枚举类</param>
        /// <returns>数组</returns>
        public static object[] ToCopyArray<T>(this IEnumerable<T> source)
        {
            return source.Select(x => ((ICloneable)x).Clone()).ToArray();
        }
    }
}

#region 去重复比较对象
/// <summary>
/// 去重复比较对象
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="V"></typeparam>
public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
{
    private Func<T, V> keySelector;

    public CommonEqualityComparer(Func<T, V> keySelector)
    {
        this.keySelector = keySelector;
    }

    public bool Equals(T x, T y)
    {
        return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
    }

    public int GetHashCode(T obj)
    {
        return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
    }
}
#endregion