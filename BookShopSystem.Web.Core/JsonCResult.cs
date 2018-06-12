using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Web.Core
{
    /// <summary>
    /// 自定义JosnResult
    /// </summary>
    public class JsonCResult<T> : ActionResult
    {

        /// <summary> Camel格式的json结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="jsonRequestBehavior">知否支持Get操作</param>
        public JsonCResult(T data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            Data = data;
            JsonRequestBehavior = jsonRequestBehavior;
        }

        public T Data { get; set; }

        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/plain";
            if (Data != null)
            {
                response.Write(JsonHelper.SerializeObject(Data));
            }
        }
    }
}
