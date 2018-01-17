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

namespace nsda.Services.Implement.admin
{
    public class OrderService: IOrderService
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
                var sql = $@"select * from t_order where isdelete=0 {join.ToString()} order by createtime desc";
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

        //订单详情
        public OrderDetailResponse Detail(int id)
        {
            OrderDetailResponse response = null;
            try
            {
                t_order order = _dbContext.Get<t_order>(id);
                if (order != null)
                {
                    response = new OrderDetailResponse {


                    };
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
                var sql = $@"select * from t_order_operation where isdelete=0 {join.ToString()} order by createtime desc";
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
                t_order_operation order_opertion = _dbContext.Get<t_order_operation>(id);
                if (order_opertion != null)
                {
                    response = new RefundOrderDetailResponse {

                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("OrderService.RefundList", ex);
            }
            return response;
        }
    }
}
