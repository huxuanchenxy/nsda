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
    }
}
