using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto
{
    public class Result<T>
    {
        public Result()
        {
            flag = false;
            data = default(T);
            msg = string.Empty;
        }

        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool flag { get; set; }

        /// <summary>
        /// 返回的提示消息
        /// </summary>
        public string msg { get; set; }
    }

    public class ResultDto<T> where T : class
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public List<T> rows { get; set; }

        /// <summary>
        /// total
        /// </summary>
        public int total { get; set; }
    }
}
