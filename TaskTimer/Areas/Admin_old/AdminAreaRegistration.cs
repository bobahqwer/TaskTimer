using System.Web.Mvc;

namespace TaskTimer.Areas.Admin
{
    public class Admin_oldAreaRegistration : AreaRegistration
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
                "My_Admin_old",
                "Admin_old/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_old_default",
                "Admin_old/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
