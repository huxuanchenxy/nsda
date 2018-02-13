using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Services.Contract.referee;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.eventmanage.Controllers
{
    public class eventmanageController : eventbaseController
    {
        IEventRoomService _eventRoomService;
        IEventPrizeService _eventPrizeService;
        IMemberService _memberService;
        IEventService _eventService;
        IMemberTempService _memberTempService;
        IEventSignService _eventSignService;
        IPlayerSignUpService _playerSignUpService;
        IRefereeSignUpService _refereeSignUpService;
        IEventCyclingRaceSettingsService _eventCyclingRaceSettingsService;
        IEventknockoutSettingsService _eventknockoutSettingsService;
        public eventmanageController(IEventknockoutSettingsService eventknockoutSettingsService,IEventCyclingRaceSettingsService eventCyclingRaceSettingsService,IEventRoomService eventRoomService,IEventPrizeService eventPrizeService,IMemberService memberService, IEventService eventService, IMemberTempService memberTempService,IEventSignService eventSignService, IPlayerSignUpService playerSignUpService,IRefereeSignUpService refereeSignUpService)
        {
            _eventknockoutSettingsService = eventknockoutSettingsService;
            _eventCyclingRaceSettingsService = eventCyclingRaceSettingsService;
            _eventRoomService = eventRoomService;
            _eventPrizeService = eventPrizeService;
            _memberService = memberService;
            _eventService = eventService;
            _memberTempService = memberTempService;
            _eventSignService = eventSignService;
            _playerSignUpService = playerSignUpService;
            _refereeSignUpService = refereeSignUpService;
        }

        #region view
        public ActionResult index()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/eventmanage/eventmanage/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }
        //赛事管理员资料页
        public ActionResult info()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var data = _memberService.MemberEventDetail(userContext.Id);
            return View(data);
        }
        //第一步
        public ActionResult start()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            return View();
        }
        //第二步
        public ActionResult next(EventTypeEm eventType, EventTypeNameEm eventTypeName)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            ViewBag.EventType = eventType;
            ViewBag.EventTypeName = eventTypeName;
            return View();
        }
        //二维码
        public ActionResult qrcode(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            return View();
        }
        //赛事组别详情
        public ActionResult eventgroup(int eventGroupId)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.EventGroupDetail(eventGroupId);
            return View(detail);
        }
        //赛事详情
        public ActionResult detail(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            return View(detail);
        }
        //选手报名页
        public ActionResult playersignup(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id,userContext.Id);
            return View(detail);
        }
        //裁判报名页
        public ActionResult refereesignup(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.RefereeData = _refereeSignUpService.RefereeData(id, userContext.Id);
            return View(detail);
        }
        //添加裁判
        public ActionResult addreferee(int id)
        {
            ViewBag.EventId = id;
            return View();
        }
        //教室管理
        public ActionResult room(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, userContext.Id);
            bool isVisiable = false;
            int roomCount= _eventRoomService.RoomCount(id,out isVisiable);
            ViewBag.RoomCount = roomCount;
            ViewBag.IsVisiable = isVisiable;
            return View(detail);
        }
        //添加教室
        public ActionResult addroom(int id)
        {
            ViewBag.EventId = id;
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, UserContext.WebUserContext.Id);
            return View();
        }
        //更新教室
        public ActionResult updateroom(int id)
        {
            var detail = _eventRoomService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, UserContext.WebUserContext.Id);
            return View(detail);
        }
        //教室设定特殊选手
        public ActionResult addroomplayer(int id)
        {
            var detail = _eventRoomService.Detail(id);
            return View(detail);
        }
        //选手签到页
        public ActionResult playersign(int eventId, int eventGroupId=0)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(eventId);
            var eventgroup = _eventService.SelectEventGroup(eventId, UserContext.WebUserContext.Id);
            ViewBag.EventGroup = eventgroup;
            ViewBag.EventDate = _eventService.EventDate(eventId);
            ViewBag.EventGroupId = eventGroupId == 0?eventgroup.FirstOrDefault().Id: eventGroupId;
            return View(detail);
        }
        //裁判签到页
        public ActionResult refereesign(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, UserContext.WebUserContext.Id);
            ViewBag.EventDate = _eventService.EventDate(id);
            ViewBag.Data = null;
            return View(detail);
        }
        //循环赛设置
        public ActionResult cyclingsetting(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, UserContext.WebUserContext.Id);
            ViewBag.CyclingRaceSettings = _eventCyclingRaceSettingsService.CyclingRaceSettings(id);
            return View(detail);
        }
        //淘汰赛设置
        public ActionResult knockoutsetting(int id)
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventGroup = _eventService.SelectEventGroup(id, UserContext.WebUserContext.Id);
            ViewBag.KnockoutSettings = _eventknockoutSettingsService.KnockoutSettings(id);
            return View(detail);
        }
        //添加临时报名选手
        public ActionResult addplayer(int eventId, int eventGroupId)
        {
            return View();
        }
        #endregion

        #region 赛事信息设置
        /// <summary>
        /// 上传图片 赛事描述
        /// </summary>
        [HttpPost]
        public ContentResult uploadimage()
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
                return Result<string>(false, "上传图片失败");
            }

            var uploadImage = new UploadFileRequest
            {
                FileEnum = FileEnum.EventAttachment,
                ExtendName = extendName,
                FileName = Path.GetFileName(files[0].FileName),
                Size = 2,
                FileBinary = uploadFileBytes
            };
            string msg = string.Empty;
            string imageUrl = CommonFileServer.UploadFile(new UploadFileRequest
            {
                ExtendName = extendName,
                FileBinary = uploadFileBytes,
                Size = 2,
                FileEnum = FileEnum.MemberHead,
                FileName = Path.GetFileName(files[0].FileName)
            }, out msg);
            if (msg.IsNotEmpty())
            {
                return Result<string>(false, "上传图片失败");
            }
            else
            {
                return Result<string>(true, "", imageUrl);
            }
        }

        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        public ContentResult edit(RegisterEventRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberEvent(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //新增赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public ContentResult insertevent(EventRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        //编辑赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public ContentResult editevent(EventRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        //编辑赛事组别信息
        [HttpPost]
        [AjaxOnly]
        public ContentResult editeventgroup(EventGroupRequest request)
        {
            string msg = string.Empty;
            var flag = _eventService.EditGroup(request, out msg);
            return Result<string>(flag, msg);
        }

        //修改赛事状态
        [HttpPost]
        [AjaxOnly]
        public ContentResult editeventstatus(int id,EventStatusEm eventStatus)
        {
            string msg = string.Empty;
            var flag = _eventService.EditEventStatus(id, eventStatus, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //赛事列表
        [HttpGet]
        public ContentResult listevent(EventQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _eventService.EventList(request);
            var res = new ResultDto<EventResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //赛事组别信息
        [HttpGet]
        public ContentResult listeventgroup(int eventId)
        {
            var data = _eventService.SelectEventGroup(eventId,UserContext.WebUserContext.Id);
            return Result(true, string.Empty, data);
        }
        #endregion

        #region 奖项设置
        [HttpPost]
        [AjaxOnly]
        public ContentResult insertprize(EventPrizeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventPrizeService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult editprize(EventPrizeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventPrizeService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        public ContentResult deleteprize(int id)
        {
            string msg = string.Empty;
            var flag = _eventPrizeService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listprize(int eventId, int eventGroupId)
        {
            var data = _eventPrizeService.List(eventId, eventGroupId);
            return Result(true, string.Empty, data);
        }
        #endregion

        #region 教室设置
        [HttpPost]
        [AjaxOnly]
        public ContentResult insertroom(EventRoomRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventRoomService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult editroom(EventRoomRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventRoomService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult editroomsettings(int id, int status)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.EidtSettings(id, status, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        public ContentResult batcheditroomsettings(List<int> id, int status)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.BatchEidtSettings(id, status, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        public ContentResult settingroomspec(int id, int memberId)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.SettingSpec(id, memberId, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult clearroomspec(int id)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.ClearSpec(id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listroom(EventRoomQueryRequest request)
        {
            var data = _eventRoomService.List(request);
            var res = new ResultDto<EventRoomResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        [HttpGet]
        public ContentResult selectplayer(int eventId,int? eventGroupId,string keyvalue)
        {
            var data = _playerSignUpService.SelectPlayer(eventId, eventGroupId,keyvalue);
            return Result(true, string.Empty, data);
        }
        #endregion

        #region 裁判设置
        //裁判报名列表
        [HttpGet]
        public ContentResult listrefereesignup(EventRefereeSignUpQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _refereeSignUpService.EventRefereeList(request);
            var res = new ResultDto<EventRefereeSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //新增临时裁判
        [HttpPost]
        [AjaxOnly]
        public ContentResult insertreferee(TempRefereeRequest request)
        {
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempReferee(request, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //裁判审核
        [HttpPost]
        [AjaxOnly]
        public ContentResult checkreferee(int id,CheckRefereeEnum checkReferee)
        {
            string msg = string.Empty;
            var flag = _refereeSignUpService.Check(id, checkReferee, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //裁判标记
        [HttpPost]
        [AjaxOnly]
        public ContentResult refereeflag(int id)
        {
            string msg = string.Empty;
            var flag = _refereeSignUpService.Flag(id,UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region 签到管理
        //批量签到
        [HttpPost]
        [AjaxOnly]
        public ContentResult batchsign(List<int> memberId, int eventId, EventSignTypeEm eventSignType)
        {
            string msg = string.Empty;
            var flag = _eventSignService.BatchSign(memberId,eventId, eventSignType, out msg);
            return Result<string>(flag, msg);
        }


        //裁判批量签到或设置组别
        [HttpPost]
        [AjaxOnly]
        public ContentResult batchrefereesign(List<int> memberId, int eventId,int status)
        {
            string msg = string.Empty;
            var flag = _eventSignService.BatchReferee(memberId, eventId, status, out msg);
            return Result<string>(flag, msg);
        }


        //批量签到
        [HttpPost]
        [AjaxOnly]
        public ContentResult playerbatchsign(List<string> groupNum, int eventId)
        {
            string msg = string.Empty;
            var flag = _eventSignService.PlayerBatchSign(groupNum, eventId, out msg);
            return Result<string>(flag, msg);
        }

        //停赛
        [HttpPost]
        [AjaxOnly]
        public ContentResult playerstop(string groupNum, int eventId,bool isStop)
        {
            string msg = string.Empty;
            var flag = _eventSignService.Stop(groupNum, eventId, isStop, out msg);
            return Result<string>(flag, msg);
        }

        //选手签到列表
        [HttpGet]
        public ContentResult playersignlist(PlayerSignQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _eventSignService.PlayerSignList(request);
            var res = new ResultDto<PlayerSignResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //裁判签到列表
        [HttpGet]
        public ContentResult refereesignlist(RefereeSignQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _eventSignService.RefereeSignList(request);
            var res = new ResultDto<RefereeSignResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //生成签到信息
        [HttpPost]
        [AjaxOnly]
        public ContentResult rendersign(int eventId)
        {
            string msg = string.Empty;
            var flag = _playerSignUpService.RenderSign(eventId, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region 选手设置
        //新增临时选手
        [HttpPost]
        [AjaxOnly]
        public ContentResult insertplayer(List<TempPlayerRequest> request)
        {
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempPlayer(request, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //选手报名列表
        [HttpGet]
        public ContentResult listplayersignup(EventPlayerSignUpQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerSignUpService.EventPlayerList(request);
            var res = new ResultDto<EventPlayerSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        #endregion

        #region 循环赛设置
        [HttpPost]
        [AjaxOnly]
        public ContentResult cyclingracesettings(List<EventCyclingRaceSettingsRequest> request)
        {
            string msg = string.Empty;
            var flag = _eventCyclingRaceSettingsService.Settints(request,out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region 淘汰赛设置
        [HttpPost]
        [AjaxOnly]
        public ContentResult knockoutsettings(List<EventknockoutSettingsRequest> request)
        {
            string msg = string.Empty;
            var flag = _eventknockoutSettingsService.Settints(request, out msg);
            return Result<string>(flag, msg);
        }
        #endregion
    }
}