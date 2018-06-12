using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Utilities
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        ///将对象/对象集合转换成Json数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="dateFormat">是否日期格式化</param>
        /// <returns>Json数据</returns>
        public static string SerializeObject(object obj, bool dateFormat = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;//空值处理

            if (dateFormat)
            {
                //日期类型默认格式化处理
                settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 将Json数据转换成对象/对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">Json数据</param>
        /// <param name="dateFormat">是否日期格式化</param>m>
        /// <returns>对象/对象集合</returns>
        public static T DeSerializeObject<T>(string json, bool dateFormat = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;//空值处理
            if (dateFormat)
            {
                //日期类型默认格式化处理
                settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
