using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.IO;
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
        IMemberExtendService _memberExtendService;
        IEventService _eventService;
        public commondataController(IProvinceService provinceService, ICityService cityService, ISchoolService schoolService, ILeavingMsgService leavingMsgService, ICountryService countryService,IVoteService voteService, IMemberService memberService, IMailService mailService, IMemberExtendService memberExtendService,IEventService eventService)
        {
            _provinceService = provinceService;
            _cityService = cityService;
            _schoolService = schoolService;
            _leavingMsgService = leavingMsgService;
            _countryService = countryService;
            _voteService = voteService;
            _memberService = memberService;
            _mailService = mailService;
            _memberExtendService = memberExtendService;
            _eventService = eventService;
        }
        // 赛事列表
        [HttpGet]
        public ContentResult listevent(PlayerOrRefereeEventQueryRequest request)
        {
            var data = _eventService.PlayerOrRefereeEvent(request);
            var res = new ResultDto<PlayerOrRefereeEventResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //国家
        [HttpGet]
        public ContentResult listcountry()
        {
            var data = _countryService.Country();
            return Result(true, string.Empty, data);
        }

        //省份
        [HttpGet]
        public ContentResult listprovince(int countryId)
        {
            var data = _provinceService.Province(countryId);
            return Result(true, string.Empty, data);
        }

        //城市
        [HttpGet]
        public ContentResult listcity(int provinceId)
        {
            var data = _cityService.City(provinceId);
            return Result(true, string.Empty, data);
        }

        //学校
        [HttpGet]
        public ContentResult listschool(int cityId)
        {
            var data = _schoolService.School(cityId);
            return Result(true, string.Empty, data);
        }

        //留言
        [HttpPost]
        [AjaxOnly]
        public ContentResult leaving(LeavingMsgRequest request)
        {
            var msg = string.Empty;
            var flag = _leavingMsgService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        //投票
        [HttpPost]
        [AjaxOnly]
        public ContentResult vote(int voteId, List<int> detailId)
        {
            var msg = string.Empty;
            var flag = _voteService.Vote(voteId, detailId, out msg);
            return Result<string>(flag, msg);
        }

        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(MemberRequest request)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else {
                request.Id = userContext.Id;
                string msg = string.Empty;
                var flag = _memberService.Edit(request, out msg);
                return Result<string>(flag, msg);
            }
        }

        //修改密码
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editpwd(string pwd)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else
            {
                string msg = string.Empty;
                var flag = _memberService.EditPwd(UserContext.WebUserContext.Id, pwd, out msg);
                return Result<string>(flag, msg);
            }
        }

        //上传头像
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult updatehead()
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files[0].ContentLength == 0 || files[0].FileName.IsEmpty())
                {
                    return Result<string>(false, "没有选择图片");
                }
                if (files[0].ContentLength > 2 * 1024 * 1024)
                {
                    return Result<string>(false, "图片大小限制2M");
                }
                string extendName = Path.GetExtension(files[0].FileName);
                if (extendName.Contains("exe") || extendName.Contains("bat"))
                {
                    return Result<string>(false, "禁止上传exe/bat文件");
                }
                byte[] uploadFileBytes = null;
                uploadFileBytes = new byte[files[0].ContentLength];
                try
                {
                    files[0].InputStream.Read(uploadFileBytes, 0, files[0].ContentLength);
                }
                catch
                {
                    return Result<string>(false, "上传头像失败");
                }
                string msg = string.Empty;
                string headUrl=CommonFileServer.UploadFile(new UploadFileRequest
                {
                    ExtendName = extendName,
                    FileBinary = uploadFileBytes,
                    Size = 2,
                    FileEnum = FileEnum.MemberHead,
                    FileName = Path.GetFileName(files[0].FileName)
                }, out msg);
                if (msg.IsNotEmpty())
                {
                    return Result<string>(false, "上传头像失败");
                }
                else {
                    var flag = _memberService.ReplaceHead(headUrl, userContext.Id);
                    if (!flag)
                    {
                        return Result<string>(false, "上传头像失败");
                    }
                    return Result<string>(true, "");
                }
            }
        }

        //站内信标记为已读
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult mark(int id)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else
            {
                string msg = string.Empty;
                var flag = _mailService.Mark(id,UserContext.WebUserContext.Id, out msg);
                return Result<string>(flag, msg);
            }
        }

        //站内信列表
        [HttpGet]
        public ContentResult mail(MailQueryRequest request)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                var res = new ResultDto<MailResponse>
                {
                    page = request.PageIndex,
                    total = request.Total,
                    records = request.Records,
                    rows = null
                };
                return Content(res.Serialize());
            }
            else
            {
                request.MemberId = UserContext.WebUserContext.Id;
                var data = _mailService.List(request);
                var res = new ResultDto<MailResponse>
                {
                    page = request.PageIndex,
                    total = request.Total,
                    records = request.Records,
                    rows = data
                };
                return Content(res.Serialize());
            }

        }

        //删除站内信
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deletemail(int id)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else
            {
                string msg = string.Empty;
                var flag = _mailService.Delete(id, UserContext.WebUserContext.Id, out msg);
                return Result<string>(flag, msg);
            }
        }

        // 申请做裁判 或者教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult apply(MemberExtendRequest request)
        {
            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                return Result<string>(false, "登录超时,请刷新页面进行登录");
            }
            else
            {
                string msg = string.Empty;
                request.MemberId = userContext.Id;
                var flag = _memberExtendService.Apply(request, out msg);
                return Result<string>(flag, msg);
            }
        }

        private  ContentResult Result<T>(bool flag, string message = "", T data = default(T))
        {
            return Content((new Result<T> { flag = flag, msg = message, data = data }).Serialize());
        }
    }
}