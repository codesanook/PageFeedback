using System.Collections.Generic;
using Facebook;
using Newtonsoft.Json;
using PageFeedback.Service.Models;
using log4net;

namespace PageFeedback.Worker
{
    public class FbQuery
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(FbQuery));

        // // GET: /Facebook/ 
        public IList<Comment> Query(string fbToken)
        {
            var commentList = new List<Comment>();
            var client = new FacebookClient(fbToken);
            dynamic postResponse = client.Get("Noonswoon/feed?fields=id&limit=100");
            foreach (dynamic post in postResponse.data)
            {
                string postId = post.id;
                _log.DebugFormat("postId [{0}]", postId);

                var url = string.Format("{0}/comments?fields=message&limit=250", postId);
                _log.DebugFormat("url [{0}]", url);

                dynamic commentResponse = client.Get(url);
                var json = JsonConvert.SerializeObject(commentResponse);
                _log.DebugFormat("commentResponse [{0}]", json);

                var comments = (JsonArray)commentResponse.data;
                if (comments.Count == 0)
                {
                    continue;
                }
                foreach (dynamic comment in comments)
                {


                    _log.DebugFormat("comment [{0}]", comment);
                    _log.DebugFormat("message [{0}]", comment.message);
                    commentList.Add(new Comment()
                    {
                         Message = comment.message,
                         Url =url
                    });
                }
            }

            return commentList;

        }


    }
}

