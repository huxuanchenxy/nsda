using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class orderController : baseController
    {
        IOrderService _orderService;
        public orderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region ajax
        //订单列表
        [HttpGet]
        public ContentResult orderlist(OrderListQueryRequest request)
        {
            var data = _orderService.List(request);
            var res = new ResultDto<OrderListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        //对单列表
        [HttpGet]
        public ContentResult refundlist(RefundOrderListQueryRequest request)
        {
            var data = _orderService.RefundList(request);
            var res = new ResultDto<RefundOrderListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //处理退款
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult process(int id)
        {
            var msg = string.Empty;
            var flag =_orderService.Process(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view
        //订单详情
        public ActionResult orderdetail(int id)
        {
            var detail = _orderService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        //退单详情
        public ActionResult refunddetail(int id)
        {
            var detail = _orderService.RedundDetail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        public ActionResult index()
        {
            return View();
        }

        public ActionResult refund()
        {
            return View();
        }
        #endregion 
    }
}