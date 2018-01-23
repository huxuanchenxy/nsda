using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class RatingSinglLabelResponse
    {
        /// <summary>
        /// 裁判名字
        /// </summary>
        public string RefereeName { get; set; }
        /// <summary>
        /// 反方队伍编号
        /// </summary>
        public string ConGroupNum { get; set; }
        /// <summary>
        /// 正方队伍编号
        /// </summary>
        public string ProGroupNum { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// FlightA FlightB FlightC
        /// </summary>
        public int Screenings { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 组别名称
        /// </summary>
        public string EventGroupName { get; set; }
        /// <summary>
        /// 第几轮
        /// </summary>
        public int Round { get; set; }
    }
}
