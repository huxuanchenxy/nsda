using nsda.Services;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Filter
{
    public class CoachLoginAttribute : AuthorizeAttribute
    {
        private LoginModeEnum _customMode;

        public CoachLoginAttribute(LoginModeEnum Mode)
        { 
            _customMode = Mode;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_customMode == LoginModeEnum.Ignore)
            {
                return;
            }

            var user = UserContext.WebUserContext;
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/login/login");
                return;
            }

            if (user.MemberType != (int)Model.enums.MemberTypeEm.教练)
            {
                if (!user.IsExtendCoach)
                {
                    filterContext.Result = new RedirectResult("/login/login");
                    return;
                }
            }
        }
    }
}