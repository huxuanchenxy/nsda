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
        public string ChinessName { get; set; }
        public string EnglishName { get; set; }
        public bool IsInter { get; set; }
    }

    public class SchoolQueryRequest:PagedQuery
    {
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public string ChinessName { get; set; }
        public string EnglishName { get; set; }
    }
}
