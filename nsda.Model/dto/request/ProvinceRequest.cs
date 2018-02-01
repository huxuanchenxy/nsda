using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class ProvinceRequest
    {
        public int Id { get; set; }
        public bool IsInter { get; set; }
        public string Name { get; set; }
    }

    public class ProvinceQueryRequest:PageQuery
    {
        public bool? IsInter { get; set; }
        public string Name { get; set; }
    }
}
