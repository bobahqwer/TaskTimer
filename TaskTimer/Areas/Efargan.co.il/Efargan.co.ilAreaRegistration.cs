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
                "Efargan.co.il/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
