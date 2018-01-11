using nsda.Model.dto.request;
using nsda.Services.admin;
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
    //基础数据维护
    public class basedataController : baseController
    {
        IProvinceService _provinceService;
        ICityService _cityService;
        IDataSourceService _dataSourceService;
        ISchoolService _schoolService;
        ILeavingMsgService _leavingMsgService;
        ICountryService _countryService;
        public basedataController(IProvinceService provinceService, ICityService cityService, IDataSourceService dataSourceService, ISchoolService schoolService, ILeavingMsgService leavingMsgService, ICountryService countryService)
        {
            _provinceService = provinceService;
            _cityService = cityService;
            _dataSourceService = dataSourceService;
            _schoolService = schoolService;
            _leavingMsgService = leavingMsgService;
            _countryService = countryService;
        }

        #region 国家
        public ActionResult indexcountry()
        {
            return View();
        }

        public ActionResult addcountry()
        {
            return View();
        }

        public ActionResult updatecountry(int id)
        {
            var detail = _countryService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deletecountry(int id)
        {
            var msg = string.Empty;
            var flag = _countryService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertcountry(CountryRequest request)
        {
            var msg = string.Empty;
            var flag = _countryService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editcountry(CountryRequest request)
        {
            var msg = string.Empty;
            var flag = _countryService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listcountry(CountryQueryRequest request)
        {
            var data = _countryService.List(request);
            return Result<string>(true, string.Empty);
        }

        #endregion
        #region 省
        public ActionResult indexprovince()
        {
            return View();
        }

        public ActionResult addprovince()
        {
            return View();
        }

        public ActionResult updateprovince(int id)
        {
            var detail = _provinceService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deleteprovince(int id)
        {
            var msg = string.Empty;
            var flag = _provinceService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertprovince(ProvinceRequest request)
        {
            var msg = string.Empty;
            var flag = _provinceService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editprovince(ProvinceRequest request)
        {
            var msg = string.Empty;
            var flag = _provinceService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listprovince(ProvinceQueryRequest request)
        {
            var data = _provinceService.List(request);
            return Result<string>(true, string.Empty);
        }

        #endregion

        #region 城市
        public ActionResult indexcity()
        {
            return View();
        }

        public ActionResult addcity()
        {
            return View();
        }

        public ActionResult updatecity(int id)
        {
            var detail = _cityService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deletecity(int id)
        {
            var msg = string.Empty;
            var flag = _cityService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertcity(CityRequest request)
        {
            var msg = string.Empty;
            var flag = _cityService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editcity(CityRequest request)
        {
            var msg = string.Empty;
            var flag = _cityService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listcity(CityQueryRequest request)
        {
            var data = _cityService.List(request);
            return Result<string>(true, string.Empty);
        }

        #endregion

        #region 学校
        public ActionResult indexschool()
        {
            return View();
        }

        public ActionResult addschool()
        {
            return View();
        }

        public ActionResult updateschool(int id)
        {
            var detail = _schoolService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deleteschool(int id)
        {
            var msg = string.Empty;
            var flag = _schoolService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertschool(SchoolRequest request)
        {
            var msg = string.Empty;
            var flag = _schoolService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editschool(SchoolRequest request)
        {
            var msg = string.Empty;
            var flag = _schoolService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listschool(SchoolQueryRequest request)
        {
            var data = _schoolService.List(request);
            return Result<string>(true, string.Empty);
        }
        #endregion

        #region 辩题资料上传
        public ActionResult indexdatasource()
        {
            return View();
        }

        public ActionResult adddatasource()
        {
            return View();
        }

        public ActionResult updatedatasource(int id)
        {
            var detail = _dataSourceService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deletedatasource(int id)
        {
            var msg = string.Empty;
            var flag = _dataSourceService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertdatasource(DataSourceRequest request)
        {
            var msg = string.Empty;
            var flag = _dataSourceService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editdatasource(DataSourceRequest request)
        {
            var msg = string.Empty;
            var flag = _dataSourceService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listdatasource(DataSourceQueryRequest request)
        {
            var data = _dataSourceService.List(request);
            return Result<string>(true, string.Empty);
        }
        #endregion

        #region 留言管理
        public ActionResult leavingmsg(int id)
        {
            var detail = _leavingMsgService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        [HttpGet]
        public ContentResult listleavingmsg(LeavingMsgQueryRequest request)
        {
            var data = _leavingMsgService.List(request);
            return Result<string>(true, string.Empty);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deleteleavingmsg(int id)
        {
            var msg = string.Empty;
            var flag = _leavingMsgService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult processleavingmsg(int id)
        {
            var msg = string.Empty;
            var flag = _leavingMsgService.Process(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion 

    }
}