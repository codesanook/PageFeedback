using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace PageFeedback.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Content("Index");
        }


        //Home/PublishMessage
        public ActionResult PublishMessage()
        {
//http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("fbqueryrequest");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(string.Format("Hello, World at {0}",DateTime.Now));
            queue.AddMessage(message);

            return Content("published");
        }
    }
}
