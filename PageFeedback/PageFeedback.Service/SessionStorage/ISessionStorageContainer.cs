using NHibernate;

namespace PageFeedback.Service.SessionStorage
{
    public interface ISessionStorageContainer
    {
        ISession GetCurrentSession();
        void Store(ISession session);
        void Clear();
    }
}
