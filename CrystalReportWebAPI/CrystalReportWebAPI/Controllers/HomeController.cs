using System.Web.Mvc;

namespace CrystalReportWebAPI.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}