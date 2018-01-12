using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class commondataController : Controller
    {
        IVoteService _voteService;
        IProvinceService _provinceService;
        ICityService _cityService;
        ISchoolService _schoolService;
        ILeavingMsgService _leavingMsgService;
        ICountryService _countryService;
        IMemberService _memberService;
        IMailService _mailService;
        public commondataController(IProvinceService provinceService, ICityService cityService, ISchoolService schoolService, ILeavingMsgService leavingMsgService, ICountryService countryService,IVoteService voteService, IMemberService memberService, IMailService mailService)
        {
            _provinceService = provinceService;
            _cityService = cityService;
            _schoolService = schoolService;
            _leavingMsgService = leavingMsgService;
            _countryService = countryService;
            _voteService = voteService;
            _memberService = memberService;
            _mailService = mailService;
        }
        //国家
        [HttpGet]
        public ContentResult listcountry()
        {
            var data = _countryService.Country();
            return Content((new Result<List<BaseDataResponse>> { flag = true,msg=string.Empty, data = data }).Serialize());
        }

        //省份
        [HttpGet]
        public ContentResult listprovince(int countryId)
        {
            var data = _provinceService.Province(countryId);
            return Content((new Result<List<BaseDataResponse>> { flag = true, msg = string.Empty, data = data }).Serialize());
        }

        //城市
        [HttpGet]
        public ContentResult listcity(int provinceId)
        {
            var data = _cityService.City(provinceId);
            return Content((new Result<List<BaseDataResponse>> { flag = true, msg = string.Empty, data = data }).Serialize());
        }

        //学校
        [HttpGet]
        public ContentResult listschool(int cityId)
        {
            var data = _schoolService.School(cityId);
            return Content((new Result<List<BaseDataResponse>> { flag = true, msg = string.Empty, data = data }).Serialize());
        }

        //留言
        [HttpPost]
        [AjaxOnly]
        public ContentResult leaving(LeavingMsgRequest request)
        {
            var msg = string.Empty;
            var flag = _leavingMsgService.Insert(request, out msg);
            return Content((new Result<string> { flag = flag, msg = msg }).Serialize());
        }

        //投票
        [HttpPost]
        [AjaxOnly]
        public ContentResult vote(int voteId, List<int> detailId)
        {
            var msg = string.Empty;
            var flag = _voteService.Vote(voteId, detailId, out msg);
            return Content((new Result<string> { flag = flag, msg = msg }).Serialize());
        }

        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(MemberRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                res.flag = false;
                res.msg = "请刷新页面进行登录";
            }
            else {
                request.Id = userContext.Id;
                res.flag = _memberService.Edit(request, out msg);
                res.msg = msg;
            }
            return Content(res.Serialize());
        }

        //修改密码
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editpwd(string oldPwd, string newPwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                res.flag = false;
                res.msg = "请刷新页面进行登录";
            }
            else
            {
                res.flag = _memberService.EditPwd(UserContext.WebUserContext.Id, oldPwd, newPwd, out msg);
                res.msg = msg;
            }
            return Content(res.Serialize());
        }

        //站内信标记为已读
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult mark(List<int> ids)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                res.flag = false;
                res.msg = "请刷新页面进行登录";
            }
            else
            {
                res.flag = _mailService.Mark(ids,UserContext.WebUserContext.Id, out msg);
                res.msg = msg;
            }
            return Content(res.Serialize());
        }

        //站内信列表
        [HttpGet]
        public ContentResult mail(MailQueryRequest request)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                var res = new Result<string>();
                string msg = string.Empty;
                res.flag = false;
                res.msg = "请刷新页面进行登录";
                return Content(res.Serialize());
            }
            else
            {
                request.MemberId = UserContext.WebUserContext.Id;
                var data = _mailService.List(request);
                return Content((new Result<PagedList<MailResponse>> { flag = true, msg = string.Empty, data = data }).Serialize());
            }

        }

        //删除站内信
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deletemail(int id)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                res.flag = false;
                res.msg = "请刷新页面进行登录";
            }
            else
            {
                res.flag = _mailService.Delete(id, UserContext.WebUserContext.Id, out msg);
                res.msg = msg;
            }
            return Content(res.Serialize());
        }
    }
}