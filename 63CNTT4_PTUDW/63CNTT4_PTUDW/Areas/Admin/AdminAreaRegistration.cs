using System.Web.Mvc;

namespace _63CNTT4_PTUDW.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller="Dashboardt", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}