using System.Web.Mvc;

namespace FSBO.MainSite.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Home()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            return PartialView();
        }

        public ActionResult Services()
        {
            return PartialView();
        }

    }
}
