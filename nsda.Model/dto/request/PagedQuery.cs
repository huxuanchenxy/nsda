using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PagedQuery
    {
        public int PageIndex { get; set; } = 1;
        public int PagesSize { get; set; } = 10;
    }
}
