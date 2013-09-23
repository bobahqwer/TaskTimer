using System.Web.Mvc;

namespace TaskTimer.Areas.mmgtaabora.com {
    public class mmgtaaboraAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "mmgtaabora.com";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "mmgtaabora.com_default",
                "mmgtaabora.com/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
