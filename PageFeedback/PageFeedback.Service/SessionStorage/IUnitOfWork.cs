using System;

namespace PageFeedback.Service.SessionStorage
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit(bool isWithTransactionRollback);
        void Commit();
        void Undo();
    }
}
