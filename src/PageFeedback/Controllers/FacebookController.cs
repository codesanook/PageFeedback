using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;

namespace PageFeedback.Controllers
{
    public class FacebookController : Controller
    {

        public ActionResult GraphApi()
        {
            var postIds = new List<string>();

            var client = new FacebookClient();
            dynamic response = client.Get("190581777753115/feed?fields=id&limit=100");
            foreach (dynamic post in response.data)
            {
                //find comment for post id
                postIds.Add(post.id);
            }

            var csv = string.Join(",", postIds);
            return Content(csv);
        }
    }
}