using System.Web.Mvc;

namespace nsda.Web.Areas.player
{
    public class playerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "player";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "player_default",
                "player/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}