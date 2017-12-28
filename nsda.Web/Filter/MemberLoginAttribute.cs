using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Filter
{
    public class MemberLoginAttribute : AuthorizeAttribute
    {
        private LoginModeEnum _customMode;

        public MemberLoginAttribute(LoginModeEnum Mode)
        {
            _customMode = Mode;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_customMode == LoginModeEnum.Ignore)
            {
                return;
            }
        }
    }
}