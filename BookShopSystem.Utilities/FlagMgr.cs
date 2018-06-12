using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Utilities
{
    /// <summary>
    /// 标记管理器
    /// </summary>
    public class FlagMgr
    {
        /// <summary>
        /// 上传
        /// </summary>
        public class Upload
        {
            /// <summary>
            /// 来源类型
            /// </summary>
            public enum SourceType
            {
                /// <summary>
                /// 图书
                /// </summary>
                Book=1
            }
        }

        /// <summary>
        /// 订单
        /// </summary>
        public class Order
        {
            /// <summary>
            /// 订单状态
            /// </summary>
            public enum OrderSateFlag
            {
                /// <summary>
                /// 取消
                /// </summary>
                Cancel=-1,
                /// <summary>
                /// 创建
                /// </summary>
                Created=0,
                /// <summary>
                /// 已支付
                /// </summary>
                Pay=1,
                /// <summary>
                /// 运输中
                /// </summary>
                Send=2,
                /// <summary>
                /// 已完成
                /// </summary>
                Finish=3
            }
        }

    }


    /// <summary>
    /// 颜色格式
    /// </summary>
    public enum ColorFormat
    {
        [Description("<span >{0}</span>")]
        Normal,
        [Description("<span  style='color:red'>{0}</span>")]
        Red,
        [Description("<span  style='color:green'>{0}</span>")]
        Green,
        [Description("<span  style='color:gray'>{0}</span>")]
        Gray,
        [Description("<span  style='color:#ff9900'>{0}</span>")]
        Orange

    }

    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的值
        /// </summary>
        /// <param name="obj">当前枚举对象</param>
        /// <returns>枚举的值</returns>
        public static int ToInt(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 根据整型获取枚举
        /// </summary>
        /// <param name="obj">当前枚举对象</param>
        /// <returns>枚举的值</returns>
        public static T ToEnum<T>(this int obj)
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }

    }

    /// <summary>
    /// 枚举管理器
    /// </summary>
    public class EnumManager
    {
        /// <summary>
        /// 获取枚举信息（键、值）
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>枚举信息（键、值）</returns>
        public static Dictionary<int, string> GetInfo(Type type)
        {
            Type descType = typeof(DescriptionAttribute);
            DescriptionAttribute attr = null;
            int value = 0;
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var field in fields)
            {
                value = (int)field.GetValue(null);
                if (field.IsDefined(descType, true))
                {
                    attr = field.GetCustomAttributes(descType, true)[0] as DescriptionAttribute;
                }
                if (!dic.ContainsKey(value))
                {
                    dic.Add(value, attr.Description);
                }
            }
            return dic;
        }

        /// <summary>
        /// 根据对应的枚举类型获取所有项的特性
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>所有项的特性</returns>
        public static List<DescriptionAttribute> GetAttributes(Type type)
        {
            List<DescriptionAttribute> list = new List<DescriptionAttribute>();
            Type descType = typeof(DescriptionAttribute);
            DescriptionAttribute attr = null;
            int value = 0;
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public).OrderBy(item => (int)item.GetValue(null));
            foreach (var field in fields)
            {
                value = (int)field.GetValue(null);
                if (value == -1) { continue; }
                if (field.IsDefined(descType, true))
                {
                    attr = field.GetCustomAttributes(descType, true)[0] as DescriptionAttribute;
                    list.Add(attr);
                }
            }
            return list;
        }
    }

    /// <summary>
    /// 枚举描述特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 包含类型
        /// </summary>
        public Type IncludeType { get; set; }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据表字段
        /// </summary>
        public string PK { get; set; }

        /// <summary>
        /// 详细页面
        /// </summary>
        public string DetailPage { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public ColorFormat Color { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Abbr { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 已审核标记
        /// </summary>
        public int HadAprrovalFlag { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        public DescriptionAttribute() { }



        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="description">描述信息</param>
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public DescriptionAttribute(string description, ColorFormat color)
            : this(description)
        {
            this.Color = color;
        }

        public DescriptionAttribute(string description, string detail, ColorFormat color)
            : this(description, color)
        {
            this.Detail = detail;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="description">描述信息</param>
        /// <param name="tableName">对应表名称</param>
        /// <param name="pk">主键</param>
        /// <param name="detailPage">展示页面</param>
        /// <param name="groupName">分组名称</param>
        public DescriptionAttribute(string description, string tableName, string pk, string detailPage, string abbr, string groupName)
        {
            this.Description = description;
            this.TableName = tableName;
            this.PK = pk;
            this.DetailPage = detailPage;
            this.Abbr = abbr;
            this.GroupName = groupName;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="description">描述信息</param>
        /// <param name="tableName">对应表名称</param>
        /// <param name="pk">主键</param>
        /// <param name="detailPage">展示页面</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="includeType">包含类型</param>
        public DescriptionAttribute(string description, string tableName, string pk, string detailPage, string abbr, string groupName, Type includeType)
            : this(description, tableName, pk, detailPage, abbr, groupName)
        {
            this.IncludeType = includeType;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="description">描述信息</param>
        /// <param name="tableName">对应表名称</param>
        /// <param name="pk">主键</param>
        /// <param name="detailPage">展示页面</param>
        /// <param name="abbr">简称</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="includeType">包含类型</param>
        /// <param name="HadApprovalFlag">已审核标记</param>
        /// <remarks>作者：dfq 时间：2016-08-27</remarks>
        public DescriptionAttribute(string description, string tableName, string pk, string detailPage, string abbr, string groupName, Type includeType, int HadApprovalFlag)
            : this(description, tableName, pk, detailPage, abbr, groupName)
        {
            this.IncludeType = includeType;
            this.HadAprrovalFlag = HadApprovalFlag;
        }
    }

    /// <summary>
    /// 枚举操作类
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    public static class Enum<T>
    {
        /// <summary>
        /// 将枚举类型转化为集合
        /// </summary>
        /// <returns>枚举项集合</returns>
        public static List<DropdownListItem> ToList()
        {
            Type type = typeof(T);
            List<DropdownListItem> list = new List<DropdownListItem>();
            Array array = Enum.GetValues(type);
            foreach (int value in array)
            {
                DropdownListItem item = new DropdownListItem();
                item.TextField = GetDesc(value);
                item.ValueField = value.ToString();
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取枚举键值对
        /// </summary>
        /// <returns>键值对</returns>
        public static Dictionary<int, string> ToDic()
        {
            Type type = typeof(T);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            Array array = Enum.GetValues(type);
            foreach (int value in array)
            {
                if (!dic.ContainsKey(value))
                {
                    dic.Add(value, GetDesc(value));
                }
                else
                {
                    dic[value] = GetDesc(value);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取枚举信息列表
        /// </summary>
        /// <returns>枚举信息列表</returns>
        public static List<EnumInfo> GetInfo()
        {
            Type type = typeof(T);
            Array array = Enum.GetValues(type);
            List<EnumInfo> list = new List<EnumInfo>();
            foreach (int value in array)
            {
                EnumInfo info = new EnumInfo();
                info.Attribute = GetAttribute(type, (T)Enum.ToObject(type, value));
                info.Value = value;
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举项</param>
        /// <returns>描述信息</returns>
        public static string GetDesc(Type type, T value)
        {
            var attr = GetAttribute(type, value);
            return attr == null ? "" : attr.Description;
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="value">枚举项</param>
        /// <returns>描述信息</returns>
        public static string GetDesc(T value)
        {
            return GetDesc(typeof(T), value);
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="value">枚举整型值</param>
        /// <returns>描述信息</returns>
        public static string GetDesc(int value)
        {
            Type type = typeof(T);
            return GetDesc(type, (T)Enum.ToObject(type, value));
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetDesc(object value)
        {
            Type type = typeof(T);
            int intValue = Convert.ToInt32(value);
            return GetDesc(type, (T)Enum.ToObject(type, intValue));
        }

        /// <summary>
        /// 获取带颜色的描述信息
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetColorDesc(object value)
        {
            Type type = typeof(T);
            int intValue = Convert.ToInt32(value);
            DescriptionAttribute attr = GetAttribute((T)Enum.ToObject(type, intValue));
            string fmt = Enum<ColorFormat>.GetDesc(attr.Color);
            return String.Format(fmt, attr.Description);
        }

        /// <summary>
        /// 获取详细页面
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>详细页面</returns>
        public static string GetDetailPage(object value)
        {
            Type type = typeof(T);
            int intValue = Convert.ToInt32(value);
            return GetAttribute((T)Enum.ToObject(type, intValue)).DetailPage;
        }

        /// <summary>
        /// 获取特性值
        /// </summary>
        /// <returns>特性值</returns>
        public static string GetTableName(T value)
        {
            var attr = GetAttribute(value);
            return attr == null ? "" : attr.TableName;
        }

        /// <summary>
        /// 获取特性标签数据
        /// </summary>
        /// <returns>标签数据</returns>
        public static string GetPK(T value)
        {
            var attr = GetAttribute(value);
            return attr == null ? "" : attr.PK;
        }

        /// <summary>
        /// 获取枚举特性
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举特性</returns>
        public static DescriptionAttribute GetAttribute(T value)
        {
            return GetAttribute(typeof(T), value);
        }

        /// <summary>
        /// 获取枚举特性
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举值</param>
        /// <returns>枚举特性</returns>
        public static DescriptionAttribute GetAttribute(Type type, T value)
        {
            FieldInfo field = type.GetField(value.ToString());
            if (!field.IsDefined(typeof(DescriptionAttribute), true))
            {
                return null;
            }
            return field.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;
        }

        /// <summary>
        /// 获取枚举类型键值下拉框列表
        /// </summary>
        /// <returns>键值下拉框列表</returns>
        /// <remarks>作者:dfq 时间:2016-08-19</remarks>
        public static List<DropdownListItem> GetEnumItemList()
        {
            Type type = typeof(T);
            List<DropdownListItem> list = new List<DropdownListItem>();
            Array array = Enum.GetValues(type);
            foreach (int value in array)
            {
                DropdownListItem item = new DropdownListItem();
                item.TextField = Enum.GetName(type, value);
                item.ValueField = value.ToString();
                list.Add(item);
            }
            return list;
        }
    }


    /// <summary>
    /// 枚举信息
    /// </summary>
    public class EnumInfo
    {
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 枚举特性
        /// </summary>
        public DescriptionAttribute Attribute { get; set; }
    }

    /// <summary>
    /// 下拉框实体
    /// </summary>
    public class DropdownListItem
    {
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string TextField { get; set; }

        /// <summary>
        /// 显示文本对应的值
        /// </summary>
        public string ValueField { get; set; }
    }
}
