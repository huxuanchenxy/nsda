using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Filter
{
    public class PlayerLoginAttribute : AuthorizeAttribute
    {
        private LoginModeEnum _customMode;

        public PlayerLoginAttribute(LoginModeEnum Mode)
        {
            _customMode = Mode;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_customMode == LoginModeEnum.Ignore)
            {
                return;
            }

            var user = UserContext.WebUserContext();
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/login/login");
                return;
            }

            if (user.MemberType != (int)Model.enums.MemberTypeEm.选手)
            {
                if (!user.Role.Contains(((int)Model.enums.MemberTypeEm.选手).ToString()))
                {
                    filterContext.Result = new RedirectResult("/login/login");
                    return;
                }
            }
        }
    }
}