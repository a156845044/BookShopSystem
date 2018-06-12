using Snowflake.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Utilities
{
    public class SysHelper
    {
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="recordCount">总记录条数</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>总页数</returns>
        public static int GetPageCount(int recordCount, int pageSize)
        {
            int pageCount = 0;
            if (recordCount > 0)
            {
                pageCount = recordCount / pageSize;
                if (recordCount % pageSize > 0)
                    pageCount++;
            }
            return pageCount;
        }

        static IdWorker snowflake;

        /// <summary>
        /// snowflake算法实例
        /// </summary>
        /// <returns></returns>
        private static IdWorker Instance()
        {
            if (snowflake == null)
                //机器ID,数据中心ID
                snowflake = new IdWorker(1, 1);
            return snowflake;
        }


        /// <summary>
        /// 获取主键编号
        /// </summary>
        /// <returns>主键编号</returns>
        public static string GetPrimaryKey()
        {
            return SysHelper.Instance().NextId().ToString();
        }
    }
}
