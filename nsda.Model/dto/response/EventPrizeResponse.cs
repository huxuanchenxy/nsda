using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventPrizeResponse
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public PrizeTypeEm PrizeType { get; set; }

        public string Name { get; set; }

        public string Num { get; set; }

        public string Remark { get; set; }

    }
}
