using System.Web.Mvc;

namespace nsda.Web.Areas.member
{
    public class memberAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "member_default",
                "member/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}