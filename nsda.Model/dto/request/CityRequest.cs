using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class CityRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
    }

    public class CityQueryRequest : PageQuery
    {
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
    }
}
