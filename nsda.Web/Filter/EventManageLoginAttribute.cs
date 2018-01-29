using nsda.Services;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Filter
{
    public class EventManageLoginAttribute : AuthorizeAttribute
    {
        private LoginModeEnum _customMode;

        public EventManageLoginAttribute(LoginModeEnum Mode)
        {
            _customMode = Mode;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_customMode == LoginModeEnum.Ignore)
            {
                return;
            }

            var userContext = UserContext.WebUserContext;
            if (userContext == null)
            {
                filterContext.Result = new RedirectResult("/login/login");
                return;
            }

            if (userContext.MemberType != (int)Model.enums.MemberTypeEm.赛事管理员)
            {
                filterContext.Result = new RedirectResult("/login/login");
                return;               
            }
        }
    }
}