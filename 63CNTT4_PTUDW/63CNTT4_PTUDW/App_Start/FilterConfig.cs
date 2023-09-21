using System.Web;
using System.Web.Mvc;

namespace _63CNTT4_PTUDW
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
