namespace PageFeedback.Service.SessionStorage
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        IUnitOfWork Create(bool forceNew);
    }
}
