using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    public interface IVoteService : IDependency
    {
        //新增投票辩题
        bool Insert(VoteRequest request,out string msg);
        //更新投票辩题
        bool Update(VoteRequest request, out string msg);
        //投票辩题详情
        VoteResponse Detail(int id);
        //删除投票辩题
        bool Delete(int id,out string msg);
        //投票辩题列表
        PagedList<VoteResponse> List(VoteQueryRequest request);
        //投票
        bool Vote(int voteId, List<int> detailId, out string msg);
    }
}
