using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class DataSourceRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public string FilePath { get; set; }
    }
    public class DataSourceQueryRequest : PagedQuery
    {
        public string Title { get; set; }
        public string Remark { get; set; }
    }
}
