using System.Web.Mvc;

namespace TaskTimer.Areas.Efargan.co.il {
    public class EfarganAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Efargan.co.il";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Efargan.co.il_default",
                "Efargan.co.il/{controller}/{action}/{lng}/{id}",
                new { action = "Index", lng = "HE", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Efargan.co.il_defaultId",
                "Efargan.co.il/{controller}/{action}/{lng}",
                new { action = "Index", lng = "HE", id = 0 }
            );
            context.MapRoute(
                "Efargan.co.il_defaultLngId",
                "Efargan.co.il/{controller}/{action}",
                new { action = "Index", lng = "HE", id = 0 }
            );
        }
    }
}
