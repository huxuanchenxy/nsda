using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;
using nsda.Utilities;
using nsda.Model.dto;
using nsda.Model.dto.response;
using System.Xml;
using nsda.Services.Contract.member;
using nsda.Services.admin;

namespace nsda.unittest
{
    /// <summary>
    /// 会员管理测试
    /// </summary>
    [TestClass]
    public class MemberServiceTest : BaseTest
    {
        private IMemberService service = AutofacContainer.Resolve<IMemberService>();
        [TestMethod]
        public void MemberList()
        {
            var request = new Model.dto.request.MemberQueryRequest
            {
                PageIndex = 1,
                PageSize = 10,
                MemberStatus = Model.enums.MemberStatusEm.已认证
            };
            var list = service.List(request);
            var res = new ResultDto<MemberResponse>
            {
                page = 1,
                total = request.Total,
                records = request.Records,
                rows = list
            };

            Console.WriteLine(JsonUtils.Serialize(res));
        }

        #region 会员注册
        //选手
        [TestMethod]
        public void RegisterPlayer()
        {
            string msg = string.Empty;
            service.RegisterMemberPlayer(new Model.dto.request.RegisterPlayerRequest {
                Account = "182212ad92401@163.com",
                Pwd = "1236ads54",
                Card="asdfas",
                CardType=Model.enums.CardTypeEm.身份证,
                Grade=Model.enums.GradeEm.fifthgrade,
                Name="asda",
                SurName="asdfasfdas",
                PlayerEdu=new Model.dto.request.PlayerEduRequest {
                    EndDate=string.Empty,
                    SchoolId=1,
                    StartDate="2017-12"
                },
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan"
            },out msg);
        }
        //教练
        [TestMethod]
        public void RegisterCoach()
        {
            string msg = string.Empty;
            service.RegisterMemberCoach(new Model.dto.request.RegisterCoachRequest
            {
                Account = "18221292401@163.com",
                Pwd = "123654",
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan"
            }, out msg);
        }
        //裁判
        [TestMethod]
        public void RegisterReferee()
        {
            string msg = string.Empty;
            service.RegisterMemberReferee(new Model.dto.request.RegisterRefereeRequest
            {
                Account = "18221292401@168.com",
                Pwd = "132131231",
                CompleteName = "上官雷",
                PinYinName = "lei",
                PinYinSurName = "上官",
                ContactAddress = "adsfa",
                ContactMobile = "12132",
                EmergencyContact = "adfsa",
                EmergencyContactMobile = "adgasfda",
                Gender = Model.enums.GenderEm.男,
                EventId = 1
            }, out msg);
        }
        //赛事管理员
        [TestMethod]
        public void RegisterEvent()
        {
            string msg = string.Empty;
            service.RegisterMemberEvent(new Model.dto.request.RegisterEventRequest
            {
                Account = "18221292401@188.com",
                Card = "12321312",
                CardType = Model.enums.CardTypeEm.身份证,
                Name = "二狗",
                SurName = "王",
                ContactAddress = "青岛",
                ContactMobile = "asdfa",
                EmergencyContact = "asdfsafds",
                EmergencyContactMobile = "asdfsa",
                Gender = Model.enums.GenderEm.男,
                Pwd = "213123",
                PinYinName = "asfdsa",
                PinYinSurName = "asdfafd"
            }, out msg);
        }
        #endregion

        #region 会员详情
        //选手
        [TestMethod]
        public void Player()
        {
            service.MemberPlayerDetail(1);
        }
        //教练
        [TestMethod]
        public void Coach()
        {
            service.MemberCoachDetail(10);
        }
        //裁判
        [TestMethod]
        public void Referee()
        {
            service.MemberRefereeDetail(11);
        }
        //赛事管理员
        [TestMethod]
        public void Event()
        {
            service.MemberEventDetail(12);
        }
        #endregion

        #region 会员编辑
        //选手
        [TestMethod]
        public void EditPlayer()
        {
            string msg = string.Empty;
            service.EditMemberPlayer(new Model.dto.request.RegisterPlayerRequest
            {
                Account = "182212ad92401@163.com",
                Pwd = "1236ads54",
                Card = "asdfas",
                CardType = Model.enums.CardTypeEm.身份证,
                Grade = Model.enums.GradeEm.fifthgrade,
                Name = "asda",
                SurName = "asdfasfdas",
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan"
            },new WebUserContext {
                 Id= 13
            }, out msg);
        }
        //教练
        [TestMethod]
        public void EditCoach()
        {
            string msg = string.Empty;
            service.EditMemberCoach(new Model.dto.request.RegisterCoachRequest
            {
                Account = "18221292401@163.com",
                Pwd = "123654",
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan",
                Id = 10
            }, new WebUserContext
            {
                Id = 10
            }, out msg);
        }
        //裁判
        [TestMethod]
        public void EditReferee()
        {
            string msg = string.Empty;
            service.EditMemberReferee(new Model.dto.request.RegisterRefereeRequest
            {
                Account = "18221292401@168.com",
                Pwd = "132131231",
                CompleteName = "上官雷",
                PinYinName = "lei",
                PinYinSurName = "上官",
                ContactAddress = "adsfa",
                ContactMobile = "12132",
                EmergencyContact = "adfsa",
                EmergencyContactMobile = "adgasfda",
                Gender = Model.enums.GenderEm.男,
                EventId = 1,
                Id = 11
            }, new WebUserContext
            {
                Id = 11
            }, out msg);
        }
        //赛事管理员
        [TestMethod]
        public void EditEvent()
        {
            string msg = string.Empty;
            service.EditMemberEvent(new Model.dto.request.RegisterEventRequest
            {
                Account = "18221292401@188.com",
                Card = "12321312",
                CardType = Model.enums.CardTypeEm.身份证,
                Name = "二狗",
                SurName = "王",
                ContactAddress = "青岛",
                ContactMobile = "asdfa",
                EmergencyContact = "asdfsafds",
                EmergencyContactMobile = "asdfsa",
                Gender = Model.enums.GenderEm.男,
                Pwd = "213123",
                PinYinName = "asfdsa",
                PinYinSurName = "asdfafd",
                Id = 12
            }, new WebUserContext
            {
                Id = 12
            }, out msg);
        }
        #endregion

        #region 会员扩展
        //选手
        [TestMethod]
        public void PlayerExtend()
        {
            string msg = string.Empty;
            service.ExtendPlayer(new Model.dto.request.RegisterPlayerRequest
            {
                Account = "182212ad92401@163.com",
                Pwd = "1236ads54",
                Card = "asdfas",
                CardType = Model.enums.CardTypeEm.身份证,
                Grade = Model.enums.GradeEm.fifthgrade,
                Name = "asda",
                SurName = "asdfasfdas",
                PlayerEdu = new Model.dto.request.PlayerEduRequest
                {
                    EndDate = string.Empty,
                    SchoolId = 1,
                    StartDate = "2017-12"
                },
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan"
            },new WebUserContext { Id=10} ,out msg);
        }
        //教练
        [TestMethod]
        public void CoachExtend()
        {
            string msg = string.Empty;
            service.ExtendCoach(new Model.dto.request.RegisterCoachRequest
            {
                Account = "18221292401@163.com",
                Pwd = "123654",
                ContactAddress = "淞沪路",
                ContactMobile = "18221292401",
                EmergencyContact = "上官",
                EmergencyContactMobile = "18221292401",
                Gender = Model.enums.GenderEm.男,
                PinYinName = "shang",
                PinYinSurName = "guan",
            }, new WebUserContext
            {
                Id = 11
            }, out msg);
        }
        //裁判
        [TestMethod]
        public void RefereeExtend()
        {
            string msg = string.Empty;
            service.ExtendReferee(new Model.dto.request.RegisterRefereeRequest
            {
                Account = "18221292401@168.com",
                Pwd = "132131231",
                CompleteName = "上官雷",
                PinYinName = "lei",
                PinYinSurName = "上官",
                ContactAddress = "adsfa",
                ContactMobile = "12132",
                EmergencyContact = "adfsa",
                EmergencyContactMobile = "adgasfda",
                Gender = Model.enums.GenderEm.男,
                EventId = 1,
                Id = 10
            }, new WebUserContext
            {
                Id = 10
            }, out msg);
        }
        #endregion

        //登录
        [TestMethod]
        public void Login()
        {
            string msg = string.Empty;
            var userContext = service.Login(new Model.dto.request.LoginRequest
            {
                Account = "zhangsan@163.com",
                Pwd = "159357",
                MemberType = Model.enums.MemberTypeEm.选手
            }, out msg);

        }

        //修改密码
        [TestMethod]
        public void EditPwd()
        {
            string msg = string.Empty;
            var flag = service.EditPwd(1, "888888", out msg);
        }

        //支付回调
        [TestMethod]
        public void CallBack()
        {
            service.CallBack(4);
        }

        //重置密码
        [TestMethod]
        public void Reset()
        {
            string msg = string.Empty;
            service.Reset(1, 1, out msg);
        }

        //通过邮箱获取用户信息
        [TestMethod]
        public void SendEmail()
        {
            string msg = string.Empty;
            int id = service.SendEmail("zhangsan@163.com", out msg);
            Console.WriteLine(id.ToString());
        }

        //强制认证
        [TestMethod]
        public void Force()
        {
            string msg = string.Empty;
            service.Force(1, 1, out msg);
        }

        //模糊搜索选手
        [TestMethod]
        public void SelectPlayer()
        {
            var data = service.SelectPlayer("code", "nsda", 1);

        }
        //模糊搜索教练
        [TestMethod]
        public void SelectCoach()
        {
            var data = service.SelectCoach("code", "nsda", 1);
        }
        //判断邮箱是否存在
        [TestMethod]
        public void IsExist()
        {
            var data = service.IsExist("zhangsan@163.com");
        }
        //去认证
        [TestMethod]
        public void GoAuth()
        {
            string msg = string.Empty;
            var data = service.GoAuth(4, out msg);
        }
    }
}
