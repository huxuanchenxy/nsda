using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventScoreResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int EventGroupId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
