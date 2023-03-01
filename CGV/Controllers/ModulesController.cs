using DatabaseIO;
using System.Web.Mvc;

namespace CGV.Controllers
{
    
    public class ModulesController : Controller
    {
        CategoryFilmDao cteD = new CategoryFilmDao();
        // GET: Modules
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult Header()
        {
            ViewBag.Cate = cteD.getAllCategoryFilm();
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}