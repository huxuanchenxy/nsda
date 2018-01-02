using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class SchoolRequest
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string Name { get; set; }
    }

    public class SchoolQueryRequest:PagedQuery
    {

    }
}
