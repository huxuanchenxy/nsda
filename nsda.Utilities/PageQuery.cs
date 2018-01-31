using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class PageQuery
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Records { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        public int Total
        {
            get
            {
                if (Records > 0)
                {
                    return Records % PageSize == 0 ? Records / PageSize : Records / PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
