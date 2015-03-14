using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using log4net;

namespace PageFeedback.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private ILog _log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            var fbAppId = CloudConfigurationManager.GetSetting("FbAppId");
            _log.DebugFormat("fbAppId {0}", fbAppId);

            ViewData["FbAppId"] = fbAppId;
            return View();
        }
    }
}
