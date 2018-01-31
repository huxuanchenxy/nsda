using nsda.Model.enums;
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
        public void Callback(int orderId, string paytransaction)
        {
            try
            {
                var order = _dbContext.Get<t_order>(orderId);
                if (order != null&&(order.orderStatus==OrderStatusEm.等待支付|| order.orderStatus == OrderStatusEm.支付失败))
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        //修改订单状态
                        order.orderStatus = OrderStatusEm.支付成功;
                        order.updatetime = DateTime.Now;
                        _dbContext.Update(order);
                        //修改支付流水信息
                        _dbContext.Execute($"update t_order_paylog set paytransaction='{paytransaction}',notifyTime='{DateTime.Now}',payStatus={(int)PayStatusEm.支付成功}  where orderId={orderId} and isdelete=0");
                        _dbContext.CommitChanges();

                        Task.Factory.StartNew(() => {
                            if (order.orderType == OrderTypeEm.实名认证)
                            {
                                _memberService.CallBack(order.memberId);
                            }
                            else if (order.orderType == OrderTypeEm.临时选手绑定)
                            {
                                _memberTempService.Callback(order.memberId, order.sourceId);
                            }
                            else if (order.orderType == OrderTypeEm.赛事报名)
                            {
                                _playerSignUpService.Callback(order.memberId, order.sourceId);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        LogUtils.LogError("PayCallBackService.CallbackTran", ex);
                        _dbContext.Rollback();
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
