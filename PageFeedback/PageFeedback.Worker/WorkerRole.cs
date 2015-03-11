using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using PageFeedback.Service;
using PageFeedback.Service.Models;
using PageFeedback.Service.NhSessionFactory;
using log4net;

namespace PageFeedback.Worker
{
    public class WorkerRole : RoleEntryPoint
    {

        private ILog _log = LogManager.GetLogger(typeof(WorkerRole));



        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("PageFeedback.Worker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            _log.Debug("woker role started");

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("PageFeedback.Worker has been started");
            SessionFactory.Init();
            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("PageFeedback.Worker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("PageFeedback.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {

                try
                {
                    Trace.TraceInformation("Working");

                    // Retrieve storage account from connection string
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                        CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

                    // Create the queue client
                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                    // Retrieve a reference to a queue
                    CloudQueue queue = queueClient.GetQueueReference("fbqueryrequest");
                    queue.CreateIfNotExists();

                    // Get the next message
                    CloudQueueMessage retrievedMessage = queue.GetMessage();
                    if (retrievedMessage != null)
                    {

                        var fbToekn = retrievedMessage.AsString;
                        _log.DebugFormat("retrievedMessage.AsString [{0}]", fbToekn);
                        //Process the message in less than 30 seconds, and then delete the message
                        queue.DeleteMessage(retrievedMessage);


                        var fbQuery = new FbQuery();
                        var comments = fbQuery.Query(fbToekn);
                        var apiHelper = new ApiHelper("http://icu4jservice.azurewebsites.net");
                        List<string[]> words = new List<string[]>();
                        foreach (var comment in comments)
                        {
                            var result = apiHelper
                                  .PostHttpForm<string[]>("icu", new IcuRequest() { sentence = comment.Message });
                            words.Add(result);
                        }
                        var flattenedWord = words.SelectMany(w => w).ToArray();
                        flattenedWord = flattenedWord.Select(w => w.Trim()).ToArray();

                        using (var session = SessionFactory.GetCurrentSession())
                        {
                            var query = session.CreateSQLQuery("TRUNCATE TABLE PageCommentWords");
                            query.ExecuteUpdate();

                            foreach (var word in flattenedWord)
                            {
                                if(string.IsNullOrEmpty(word))
                                {
                                    continue;
                                }
                                session.Save(new PageCommentWord() { Word = word });
                            }
                            session.Flush();
                            _log.DebugFormat("save split word count [{0}]", words.Count());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }


                await Task.Delay(1000);
            }
        }
    }


    public class IcuRequest
    {
        public string sentence { get; set; }
    }
}
