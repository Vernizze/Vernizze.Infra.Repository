using System;
using System.Data;

namespace Vernizze.Infra.Repository.Interfaces.Base
{
    public interface IUnitOfWork
        : IDisposable
    {
        void OpenConnection();
        void Begin();
        void Commit();
        void Rollback();

        IDbConnection Connection { get; }

        IBaseRepository GetRepository(string table_name);
    }
}
