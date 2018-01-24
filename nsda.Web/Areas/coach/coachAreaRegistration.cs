using System.Web.Mvc;

namespace nsda.Web.Areas.coach
{
    public class coachAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "coach";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "coach_default",
                "coach/{controller}/{action}/{id}",
                new { action = "index", id = UrlParameter.Optional }
            );
        }
    }
}