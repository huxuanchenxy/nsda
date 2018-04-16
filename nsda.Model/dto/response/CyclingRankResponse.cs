using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class CyclingRankResponse
    {
        public int sysRank { get; set; }
        public string groupNum { get; set; }
        public int selfWin { get; set; }
        public int selfPoint { get; set; }
        public int againstWin { get; set; }
        public int againstPoint { get; set; }
        public int selfRank { get; set; }
        
    }


}
