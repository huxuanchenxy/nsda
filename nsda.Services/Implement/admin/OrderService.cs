using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.enums;

namespace nsda.Services.Implement.admin
{
    public class OrderService : IOrderService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public OrderService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        //订单列表
        public List<OrderListResponse> List(OrderListQueryRequest request)
        {
            List<OrderListResponse> list = new List<OrderListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (b.code like @KeyValue or b.account like @KeyValue) ");
                }
                if (request.OrderType.HasValue && request.OrderType > 0)
                {
                    join.Append(" and a.orderType=@OrderType ");
                }
                if (request.OrderStatus.HasValue && request.OrderStatus > 0)
                {
                    join.Append(" and a.orderStatus=@OrderStatus ");
                }
                if (request.StartDate.HasValue)
                {
                    join.Append(" and a.createtime >= @StartDate");
                }
                if (request.EndDate.HasValue)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and a.createtime<=@EndDate");
                }
                var sql = $@"select a.*,b.code MemberCode,b.account MemberAccount from t_order a
                             inner join t_member b on a.memberId=b.id
                             where a.isdelete=0 {join.ToString()} order by a.createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<OrderListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.List", ex);
            }
            return list;
        }
        //订单详情 （平台管理员和选手）
        public OrderDetailResponse Detail(int id)
        {
            OrderDetailResponse response = null;
            try
            {
                t_order order = _dbContext.Get<t_order>(id);
                if (order != null)
                {
                    response = new OrderDetailResponse
                    {
                        CreateTime = order.createtime,
                        Id = order.id,
                        IsNeedInvoice = order.isNeedInvoice,
                        MainOrderId = order.mainOrderId,
                        MemberId = order.memberId,
                        Money = order.money,
                        OrderStatus = order.orderStatus,
                        OrderType = order.orderType,
                        PayExpiryDate = order.payExpiryDate,
                        Remark = order.remark,
                        TotalCoupon = order.totalcoupon,
                        TotalDiscount = order.totaldiscount,
                        SourceId = order.sourceId,
                        UpdateTime = order.updatetime
                    };
                    var data = _dbContext.Select<t_order_detail>(c => c.orderId == id).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            response.OrderDetail.Add(new OrderDetail
                            {
                                Id = item.id,
                                Coupon = item.coupon,
                                UpdateTime = item.updatetime,
                                CreateTime = item.createtime,
                                DiscountPrice = item.discountprice,
                                MemberId = item.memberId,
                                Money = item.money,
                                Name = item.name,
                                Number = item.number,
                                OrderId = item.orderId,
                                ProductId = item.productId,
                                UnitPrice = item.unitprice
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.RefundList", ex);
            }
            return response;
        }
        //退单列表
        public List<RefundOrderListResponse> RefundList(RefundOrderListQueryRequest request)
        {
            List<RefundOrderListResponse> list = new List<RefundOrderListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (c.code like @KeyValue or c.account like @KeyValue) ");
                }
                if (request.OrderOperType.HasValue && request.OrderOperType > 0)
                {
                    join.Append(" and a.orderType=@OrderType ");
                }
                if (request.StartDate.HasValue)
                {
                    join.Append(" and a.createtime >= @StartDate");
                }
                if (request.EndDate.HasValue)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and a.createtime<=@EndDate");
                }
                var sql = $@"select a.id,a.orderId,a.title,a.content,a.orderOperType,a.createtime,a.operationStatus, 
                            b.money,b.remark,c.code MemberCode,c.account MemberAccount
                            from t_order_operation  a
                            inner join t_order b on a.orderId=b.id
                            inner join t_member c on b.memberId=c.id
                            where a.isdelete=0 {join.ToString()} order by a.createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<RefundOrderListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.RefundList", ex);
            }
            return list;
        }
        //退单详情
        public RefundOrderDetailResponse RedundDetail(int id)
        {
            RefundOrderDetailResponse response = null;
            try
            {
                var sql = $@"select a.id,a.orderId,a.title,a.content,a.orderOperType,a.createtime,a.operationStatus, 
                            b.money,b.remark
                            from t_order_operation  a
                            inner join t_order b on a.orderId=b.id
                            where a.isdelete=0 and a.id={id}";
                response = _dbContext.QueryFirstOrDefault<RefundOrderDetailResponse>(sql);
                if (response != null)
                {
                    var data = _dbContext.Select<t_order_detail>(c => c.orderId == response.OrderId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            response.OrderDetail.Add(new OrderDetail
                            {
                                Id = item.id,
                                Coupon = item.coupon,
                                UpdateTime = item.updatetime,
                                CreateTime = item.createtime,
                                DiscountPrice = item.discountprice,
                                MemberId = item.memberId,
                                Money = item.money,
                                Name = item.name,
                                Number = item.number,
                                OrderId = item.orderId,
                                ProductId = item.productId,
                                UnitPrice = item.unitprice
                            });
                        }
                    }

                    var paylog = _dbContext.QueryFirstOrDefault<t_order_paylog>($"select * from t_order_paylog where isdelete=0 and orderId={response.OrderId}");
                    if (paylog != null)
                    {
                        response.OrderPayLogDetail = new OrderPayLog
                        {
                            Id = paylog.id,
                            ActualAmount = paylog.actualAmount,
                            PaymentAmount = paylog.paymentAmount,
                            NotifyExt = paylog.notifyExt,
                            NotifyTime = paylog.notifyTime,
                            PaymentFee = paylog.paymentFee,
                            PayStatus = paylog.payStatus,
                            OrderId = paylog.orderId,
                            Paytransaction = paylog.paytransaction,
                            PayTime = paylog.payTime,
                            PayType = paylog.payType,
                            MemberId = paylog.memberId
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.RefundList", ex);
            }
            return response;
        }
        //处理退款
        public bool Process(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_order_operation order_operation = _dbContext.Get<t_order_operation>(id);
                if (order_operation != null)
                {
                    if (order_operation.operationStatus == OperationStatusEm.待处理)
                    {
                        msg = "状态已改变请刷新页面后重试";
                        return flag;
                    }
                    try
                    {
                        _dbContext.BeginTransaction();
                        order_operation.updatetime = DateTime.Now;
                        order_operation.operationStatus = OperationStatusEm.已处理;
                        _dbContext.Update(order_operation);

                        t_order torder = _dbContext.Get<t_order>(order_operation.orderId);
                        torder.updatetime = DateTime.Now;
                        torder.orderStatus = OrderStatusEm.退款成功;
                        _dbContext.Update(torder);
                        _dbContext.CommitChanges();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("OrderService.ProcessTran", ex);
                    }
                }
                else
                {
                    msg = "未找到退款申请";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("OrderService.Process", ex);
            }
            return flag;
        }
        //修改订单状态
        public void UpdateStatus(int id,OrderStatusEm orderStatus)
        {
            try
            {
                t_order order = _dbContext.Get<t_order>(id);
                if (order != null)
                {
                    order.updatetime = DateTime.Now;
                    order.orderStatus = orderStatus;
                    _dbContext.Update(order);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.UpdateStatus", ex);
            }
        }
        //订单主表详情
        public OrderResponse OrderDetail(int id)
        {
            OrderResponse response = null;
            try
            {
                t_order order = _dbContext.Get<t_order>(id);
                if (order != null)
                {
                    response = new OrderResponse
                    {
                        CreateTime = order.createtime,
                        Id = order.id,
                        IsNeedInvoice = order.isNeedInvoice,
                        MainOrderId = order.mainOrderId,
                        MemberId = order.memberId,
                        Money = order.money,
                        OrderStatus = order.orderStatus,
                        OrderType = order.orderType,
                        PayExpiryDate = order.payExpiryDate,
                        Remark = order.remark,
                        TotalCoupon = order.totalcoupon,
                        TotalDiscount = order.totaldiscount,
                        SourceId = order.sourceId,
                        UpdateTime = order.updatetime
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.OrderDetail", ex);
            }
            return response;
        }
        //支付记录
        public void PayLog(int orderId, decimal orderMoney, PayTypeEm payType, int memberId)
        {
            try
            {
                _dbContext.BeginTransaction();
                _dbContext.Execute($"update t_order_paylog set isdelete=1 where orderId={orderId} and memberId={memberId}");
                _dbContext.Insert(new t_order_paylog
                {
                    paymentAmount = orderMoney,
                    memberId = memberId,
                    orderId = orderId,
                    payTime = DateTime.Now,
                    payStatus = PayStatusEm.等待支付,
                    payType = payType
                });
                _dbContext.CommitChanges();
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("OrderService.PayLog", ex);
            }
        }
    }
}
