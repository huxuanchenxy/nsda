using System.Web.Mvc;

namespace nsda.Web.Areas.referee
{
    public class refereeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "referee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "referee_default",
                "referee/{controller}/{action}/{id}",
                new { action = "index", id = UrlParameter.Optional }
            );
        }
    }
}