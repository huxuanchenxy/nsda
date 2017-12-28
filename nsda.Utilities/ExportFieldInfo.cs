using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class ExportFieldInfo
    {
        /// <summary>
        /// 数据类型，用于强制转换，并进行格式化,其实利用反射也可以获取到数据类型，此处因为要处理Date和Date的显示格式
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 中文名，用于导出标题
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 字段名，用于反射获取值
        /// </summary>
        public string FieldName { get; set; }
    }
}
