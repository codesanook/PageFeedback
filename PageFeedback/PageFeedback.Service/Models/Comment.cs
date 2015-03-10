namespace PageFeedback.Service.Models
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual string Url { get; set; }
        public virtual string Message { get; set; }
    }
}
