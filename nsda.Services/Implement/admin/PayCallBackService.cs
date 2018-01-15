using nsda.Models;
using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.member;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 支付回调
    /// </summary>
    public class PayCallBackService: IPayCallBackService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        IPlayerSignUpService _playerSignUpService;
        IMemberService _memberService;
        IMemberTempService _memberTempService;
        public PayCallBackService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService, IPlayerSignUpService playerSignUpService,IMemberService memberService, IMemberTempService memberTempService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
            _playerSignUpService = playerSignUpService;
            _memberService = memberService;
            _memberTempService = memberTempService;
        }

        // 支付回调
        public void Callback(string json)
        {
            try
            {
                int id = 0;
                var order = _dbContext.Get<t_order>(id);
                if (order != null)
                {
                    switch (order.orderType)
                    {
                        case Model.enums.OrderTypeEm.实名认证:
                            _memberService.CallBack(order.memberId);
                            break;
                        case Model.enums.OrderTypeEm.临时选手绑定:
                            _memberTempService.Callback(order.memberId,order.sourceId);
                            break;
                        case Model.enums.OrderTypeEm.赛事报名:
                            _playerSignUpService.Callback(order.memberId,order.sourceId);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PayCallBackService.Callback", ex);
            }
        }
    }
}
