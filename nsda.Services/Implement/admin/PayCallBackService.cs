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
        public PayCallBackService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService, IPlayerSignUpService playerSignUpService,IMemberService memberService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
            _playerSignUpService = playerSignUpService;
            _memberService = memberService;
        }

        public void Callback(string json)
        {
            try
            {
                //1.0 解析查出订单号
                //2.0 查询订单详情
                //3.0 根据订单类型不同回调 认证回调 报名回调 
                _memberService.CallBack(1);
                _playerSignUpService.Callback(1);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PayCallBackService.Callback", ex);
            }
        }
    }
}
