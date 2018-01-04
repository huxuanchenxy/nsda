using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class SchoolResponse
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string ChinessName { get; set; }
        public string EnglishName { get; set; }
        public bool isInter { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
    }
}
