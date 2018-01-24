using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class UploadFileRequest
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileEnum FileEnum { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FileBinary { get; set; }

        /// <summary>
        /// 限制大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string ExtendName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }

    public enum FileEnum
    {
        /// <summary>
        /// 评分单
        /// </summary>
        [Description("评分单")]
        EventScoreAttachment = 1,

        /// <summary>
        /// 资料附件
        /// </summary>
        [Description("资料附件")]
        DataAttachment = 2,
        /// <summary>
        /// 赛事附件
        /// </summary>
        [Description("赛事附件")]
        EventAttachment = 3,

        /// <summary>
        /// 会员头像
        /// </summary>
        [Description("会员头像")]
        MemberHead = 4
    }

}
