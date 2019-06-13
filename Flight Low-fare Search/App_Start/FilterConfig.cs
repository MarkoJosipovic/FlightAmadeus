using System.Web;
using System.Web.Mvc;

namespace Flight_Low_fare_Search
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
