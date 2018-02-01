using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class DataSourceRequest
    {
        public int SysUserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public FileTypeEm  FileType { get; set; }
    }
    public class DataSourceQueryRequest : PageQuery
    {
        public string Title { get; set; }
    }
}
