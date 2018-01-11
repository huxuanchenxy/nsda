using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class CountryRequest
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class CountryQueryRequest:PagedQuery
    {
        public string Name { get; set; }
    }
}
