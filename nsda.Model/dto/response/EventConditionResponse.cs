using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventConditionResponse
    {
        public bool IsInter { get; set; }
        public int CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
    }
}
