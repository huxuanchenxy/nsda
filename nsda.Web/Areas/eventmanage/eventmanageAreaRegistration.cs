using System.Web.Mvc;

namespace nsda.Web.Areas.eventmanage
{
    public class eventmanageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "eventmanage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "eventmanage_default",
                "eventmanage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}