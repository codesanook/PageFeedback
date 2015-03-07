using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageFeedback.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();//Index.cshtml > Views/Home/
        }

        public ActionResult BecauseOfYou()
        {
            return Content("because of you");
        }
    }
}