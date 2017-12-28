using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace nsda.Utilities
{
    public static class JsonUtils
    {
        #region JsonSerializerSettings

        //自定义序列化参数
        private static readonly JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings()
        {
            /*
             * 空值的处理
             */
            NullValueHandling = NullValueHandling.Include,
            /*
             * 默认值的处理
             * Include              包括其中的成员序列化对象时，该成员的值是一样的成员的默认值。包括成员被写入到JSON。反序列化时没有任何效果。
             * Ignore               忽略其中的成员序列化对象时，这样是不会写入到JSON的成员值是相同的成员的默认值。此选项将忽略所有默认值（如为null的对象和可为空typesl;0为整数，小数和浮点数，和假的布尔值）。忽略该默认值可以通过将DefaultValueAttribute上的属性来改变。
             * Populate             大家提供一个默认值，但没有将JSON反序列化时被设置为默认值。
             * IgnoreAndPopulate    忽略其中的成员时，反序列化序列化对象和集合成员时，它们的默认值的成员值是相同的成员的默认值。
             */
            DefaultValueHandling = DefaultValueHandling.Include,
            /*
             * 时间格式化方式
             * IsoDateFormat       日期都写在ISO 8601格式，例如 "2012-03-21T05：40Z"。
             * MicrosoftDateFormat 日期写在微软JSON格式，例如 "1198908717056"。
             */
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            /*
             * 获取或设置如何循环引用（例如一个类引用本身）的处理。
             * Error	    遇到一个循环时抛出JsonSerializationException。
             * Ignore	    忽略循环引用和不序列化。
             * Serialize	序列化循环引用。
             */
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            /*
            * 设置或获取对象引用是如何被序列化保存。
            * None     序列化类型时不保留引用。
            * Objects  序列化到JSON对象的结构时保留引用。
            * Arrays   序列化到JSON数组结构时保留引用。
            * All      序列化时保留引用。
            */
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            /*
             * 表示JSON文本输出的格式。
             * None	没有特殊格式被应用。这是默认的。
             * Indented	导致子对象根据压痕和IndentChar设置缩进。
             */
            Formatting = Newtonsoft.Json.Formatting.None,
            /*
             * 获取或设置读取JSON时所使用的区域语言
             */
            Culture = new CultureInfo("zh-CN"),
            /*
             * 获取或设置日期时间时区是如何序列化和反序列化过程中的操作。
             * Local            把为本地时间。如果DateTime对象表示协调世界时（UTC），它被转换为本地时间。
             * Utc	 	        当作一个UTC时间。如果DateTime对象表示本地时间，它会转换为UTC。
             * Unspecified	    如果一个DateTime被转换为字符串,当作一个本地时间；如果将一个字符串转换为日期时间，转换为本地时间。
             * RoundtripKind	时区信息，应在转换时予以保留。
             */
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            /*
             * 获取或设置日期格式的字符串如何在阅读时的JSON解析。
             * None             格式化的日期字符串不被解析为日期类型和读取为字符串。
             * DateTime         格式化的日期字符串，被解析为DateTime。
             * DateTimeOffset   格式化的日期字符串，被解析为DateTimeOffset。
             */
            DateParseHandling = DateParseHandling.DateTime,
            /*
             * 获取或设置DateTime和DateTimeOffset值是如何编写的JSON文本格式时。
             */
            DateFormatString = @"yyyy-MM-dd HH:mm:ss",
            /*
             * 获取或设置如何丢失的成员（如JSON包含一个属性，它是不是在对象上的成员）在反序列化过程中的处理方式。
             * Ignore  忽略缺少的成员，不要试图反序列化。
             * Error   不忽略缺少的成员，抛出JsonSerializationException。
             */
            MissingMemberHandling = MissingMemberHandling.Ignore,
            /*
             * 获取或设置类型名称写入和读取是由串行处理。
             * 	None	序列化类型时不包括的。NET类型名称。
             *  Objects	序列化到JSON对象结构时，包括。NET类型名称。
             *  Arrays	序列化到JSON数组结构时，包括。NET类型名称。
             *  All	 	序列化时总是包含。NET类型名称。
             *  Auto	包括。NET类型的名字，当被序列化的对象的类型是不一样的它声明的类型。
             */
            TypeNameHandling = TypeNameHandling.None,
            /*
             * 获取或设置对象是如何反序列化过程中创建的。
             * Auto	    重用现有对象，在需要的时候创建新的对象。
             * Reuse	只重用现有对象。
             * Replace	总是创建新的对象。
             */
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            /*
             * 获取或设置字符串是如何编写的JSON文本时。
             * Default	        只有控制字符（如换行符）被转义。
             * EscapeNonAscii	所有非ASCII和控制字符（如换行符）被转义。
             * EscapeHtml		HTML（<，>，＆，'，“）和控制字符（如换行符）被转义。
             */
            StringEscapeHandling = StringEscapeHandling.Default,
            /*
             * 获取或设置构造函数是如何反序列化过程中使用。
             * Default	                        第一次尝试使用公共默认构造函数，然后回落到单paramatized构造函数，那么非公共默认构造函数。
             * AllowNonPublicDefaultConstructor	Json.NET将回落至一个paramatized构造函数之前使用非公共的默认构造函数。
             */
            ConstructorHandling = ConstructorHandling.Default,
            /*
             * 获取或设置多么特殊的浮点数字，如NaN，则PositiveInfinity和NegativeInfinity，都写为JSON。
             * String	     写特殊的浮点值作为字符串JSON中，如“非数字”，“无限”，“无穷大”。
             * Symbol		 写特殊的浮点值，如JSON的符号，如NaN的，无穷远，负无穷大。请注意，这会产生非有效的JSON。
             * DefaultValue	 写特殊的浮点值作为JSON属性的默认值，如0.0对于双属性，一个空<T>属性为null。
             */
            FloatFormatHandling = FloatFormatHandling.String,
            /*
             * 在阅读文本的JSON解析时,获取或设置点如何浮点数，例如1.0和9.9。
             * 	Double	浮点数被解析为Double。
             *  Decimal	浮点数被解析为十进制。
             */
            FloatParseHandling = FloatParseHandling.Double,
            /*
             * 获取或设置一个怎样的类型名称组件的书面和解决的序列化程序。
             * Full     在完全模式下，反序列化过程中使用的组件必须序列化过程中使用的组件完全匹配。 Assembly类的Load方法来加载程序集。
             * Simple   在简单模式下，反序列化过程中使用的组件不需要序列化过程中使用的程序集完全匹配。具体地，版本号不需要匹配的LoadWithPartialName方法用于加载程序集。
             */
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
        };

        #endregion JsonSerializerSettings

        #region 将object序列化为JSON

        /// <summary>
        /// 将object序列化为JSON
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>序列化后的JSON字符串</returns>
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj, _JsonSerializerSettings);
        }

        #endregion 将object序列化为JSON

        #region 将XML序列化为JSON

        public static string Serialize(this XmlNode obj)
        {
            return JsonConvert.SerializeXmlNode(obj, Newtonsoft.Json.Formatting.None, true);
        }

        #endregion 将XML序列化为JSON

        #region 将字符串反序列化为指定的T类

        /// <summary>
        /// 将字符串反序列化为指定的T类
        /// </summary>
        /// <typeparam name="T">需要反序列化的为的指定T类</typeparam>
        /// <param name="str">JSON字符串</param>
        /// <returns>返回反序列话后的数据类型T</returns>
        public static T Deserialize<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str, _JsonSerializerSettings);
        }

        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

        #endregion 将字符串反序列化为指定的T类
    }
}
