using System.Collections.Generic;
using System.Data;
using Vernizze.Infra.Repository.Abstract;

namespace Vernizze.Infra.Repository.Interfaces.Base
{
    public interface IBaseRepository
    {
        void Init(IDbConnection sqlConnection);
        void Init(IDbTransaction sqlTransaction);

        string TableName { get; }

        bool Insert<T>(T value) where T : BaseDataObject;
        IEnumerable<T> Get<T>() where T : BaseDataObject;
        T GetById<T>(string Id) where T : BaseDataObject;
        T GetByRefCode<T>(int RefCode) where T : BaseDataObject;
        bool DeleteAll();

        int GetTableLines();
        bool BulkAdd<T>(IEnumerable<T> values) where T : BaseDataObject;
    }
}
