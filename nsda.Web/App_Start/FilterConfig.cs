using nsda.Web.Filter;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CommonExceptionFilter());
        }
    }
}
