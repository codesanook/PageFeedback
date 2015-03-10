using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageFeedback.Web.Controllers
{
    public class FbController : Controller
    {
        //
        // GET: /Fb/


        public ActionResult Token()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Token(string fbToken)
        {
            Session["fbToken"] = fbToken;
            return Content(fbToken);
        }


        public ActionResult CurrentToken()
        {
            string fbToken = null;
            if (Session["fbToken"] != null)
            {
                fbToken = (string)Session["fbToken"];
            }
            return Content(fbToken);

        }


    }
}
