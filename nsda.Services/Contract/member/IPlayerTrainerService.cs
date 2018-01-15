using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.member
{
    /// <summary>
    /// 会员教练管理
    /// </summary>
    public interface IPlayerTrainerService : IDependency
    {
        //1.0 绑定教练 新增 修改 删除 是否同意
        bool Insert(PlayerTrainerRequest request,out string msg);
        bool Edit(PlayerTrainerRequest request,out string msg);
        bool Delete(int id, int memberId,out string msg);
        bool IsAppro(int id, bool isAppro, int memberId, out string msg);
        //3.0 教练查询 学生列表
        List<PlayerTrainerResponse> TrainerList(PlayerTrainerQueryRequest request);
        //4.0 会员查询 教练列表
        List<PlayerTrainerResponse> MemberList(PlayerTrainerQueryRequest request);
    }
}
