using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    //赛事管理
    public class admineventmgController : adminbaseController
    {
        IEventService _eventService;
        IEventRuleService _eventRuleService;
        IPlayerSignUpService _playerSignUpService;
        IEventScoreService _eventScoreService;
        public admineventmgController(IEventService eventService, IEventRuleService eventRuleService, IPlayerSignUpService playerSignUpService, IEventScoreService eventScoreService)
        {
            _eventService = eventService;
            _eventRuleService = eventRuleService;
            _playerSignUpService = playerSignUpService;
            _eventScoreService = eventScoreService;
        }

        //设定赛事等级
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult settinglevel(int id, EventLevelEm eventLevel)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.SettingLevel(id,eventLevel,UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //审核赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult check(int id,bool isAgree)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.Check(id, isAgree, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //赛事列表
        [HttpGet]
        public ContentResult listevent(EventAdminQueryRequest request)
        {
            var data = _eventService.AdminEventList(request);
            var res = new ResultDto<EventResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //审核赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult applyrefund(int eventId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.ApplyRefund(eventId, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        #region 赛事规则

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult cyclingracerule(CyclingRaceRuleRequest request)
        {
            request.SysUserId=UserContext.SysUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRuleService.CyclingRaceRule(request, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult knockoutrule(KnockoutRuleRequest request)
        {
            request.SysUserId = UserContext.SysUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRuleService.KnockoutRule(request, out msg);
            return Result<string>(flag, msg);
        }

        public ActionResult knockoutrule(int eventId)
        {
            var data = _eventRuleService.KnockoutRuleDetail(eventId);
            return View(data);
        }

        public ActionResult cyclingracerule(int eventId)
        {
            var data = _eventRuleService.CyclingRaceRuleDetail(eventId);
            return View(data);
        }
        #endregion

        #region 赛事评分单
        //赛事评分
        [HttpGet]
        public ContentResult listeventscore(int eventId, int eventGroupId)
        {
            var data = _eventScoreService.List(eventId,eventGroupId);
            return Result(true, string.Empty, data);
        }
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deleteeventscroe(int id)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventScoreService.Delete(id,UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult inserteventscroe(EventScoreRequest request)
        {
            request.SysUserId = UserContext.SysUserContext.Id;
            if (request.Title.IsEmpty())
            {
                return Result<string>(false, "请输入文件标题");
            }
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files[0].ContentLength == 0 || files[0].FileName.IsEmpty())
            {
                return Result<string>(false, "没有选择文件");
            }
            if (files[0].ContentLength > 5 * 1024 * 1024)
            {
                return Result<string>(false, "文件大小限制5M");
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
                return Result<string>(false, "上传文件失败");
            }
            string msg = string.Empty;
            string filePath = CommonFileServer.UploadFile(new UploadFileRequest
            {
                ExtendName = extendName,
                FileBinary = uploadFileBytes,
                Size = 5,
                FileEnum = FileEnum.MemberHead,
                FileName = Path.GetFileName(files[0].FileName)
            }, out msg);
            if (msg.IsNotEmpty())
            {
                return Result<string>(false, "上传文件失败");
            }
            else
            {
                request.FilePath = filePath;
                var insertmsg = string.Empty;
                var flag = _eventScoreService.Insert(request,out insertmsg);
                return Result<string>(flag, insertmsg);
            }
        }
        #endregion 
    }
}