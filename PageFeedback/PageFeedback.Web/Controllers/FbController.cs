using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NHibernate.Transform;
using Newtonsoft.Json;
using PageFeedback.Service.NhSessionFactory;
using log4net;

namespace PageFeedback.Web.Controllers
{
    public class FbController : Controller
    {

        private ILog log = LogManager.GetLogger(typeof(FbController));
        //
        // GET: /Fb/
        public ActionResult PublishMessage()
        {
            try
            {
                //http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/

                // Retrieve storage account from connection string
                var connectionString = CloudConfigurationManager.GetSetting("AzureStorageConnectionString");
                log.DebugFormat("connectionString {0}", connectionString);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
               

                

                // Create the queue client
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                // Retrieve a reference to a queue
                CloudQueue queue = queueClient.GetQueueReference("fbqueryrequest");

                // Create the queue if it doesn't already exist
                queue.CreateIfNotExists();
                // Create a message and add it to the queue.

                string fbToken = null;
                if (Session["fbToken"] != null)
                {
                    fbToken = (string)Session["fbToken"];
                }


                if (string.IsNullOrEmpty(fbToken))
                {
                    return RedirectToAction("token", "fb");
                }
                else
                {
                    var message = new CloudQueueMessage(fbToken);
                    queue.AddMessage(message);
                    return Content("published");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Content(ex.Message);
            }
          
        }

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

        public ActionResult Summary()
        {
            using (var session = SessionFactory.GetCurrentSession())
            {
                var sql = @"SELECT Word, count(Id) AS WordCount FROM PageCommentWords GROUP BY Word ORDER BY count(Id) DESC";
                var query = session.CreateSQLQuery(sql);
                query.SetResultTransformer(Transformers.AliasToBean<PageCommentWordCount>());
                var result = query.List<PageCommentWordCount>();

                var total = result.Sum(r => r.WordCount);
                //hint
                ViewData["result"] = result;
                return Content(JsonConvert.SerializeObject(new { TotalWords = total, Words = result, }, Formatting.Indented), "application/json");
            }

        }


    }

    public class PageCommentWordCount
    {
        public string Word { get; set; }
        public int WordCount { get; set; }
    }

}
