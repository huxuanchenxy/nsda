using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    public interface IOrderService: IDependency
    {
        //订单列表
        List<OrderListResponse> List(OrderListQueryRequest request);
        //订单详情
        OrderDetailResponse Detail(int id);
        //退单列表
        List<RefundOrderListResponse> RefundList(RefundOrderListQueryRequest request);
        //退单详情
        RefundOrderDetailResponse RedundDetail(int id);
        //处理退款
        bool Process(int id, int sysUserId, out string msg);
        //更改订单状态
        void UpdateStatus(int id);
        OrderResponse OrderDetail(int id);
        //插入支付记录
        bool PayLog(int orderId, decimal orderMoney, PayTypeEm payType, int memberId, out string msg);
    }
}
