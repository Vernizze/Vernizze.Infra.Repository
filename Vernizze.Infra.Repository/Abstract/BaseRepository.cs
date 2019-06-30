using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Vernizze.Infra.Repository.Interfaces.Base;
using Vernizze.Infra.Repository.Utils;

namespace Vernizze.Infra.Repository.Abstract
{
    public abstract class BaseRepository
        : IBaseRepository
    {
        #region Variables

        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection;

        protected string _tableName = string.Empty;

        #endregion  

        #region Attributes

        public string TableName { get { return this._tableName; } }

        #endregion  

        #region Methods

        public abstract bool Insert<T>(T value) where T : BaseDataObject;

        public virtual void Init(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }

        public virtual void Init(IDbTransaction dbTransaction)
        {
            this._dbTransaction = dbTransaction;

            this._dbConnection = this._dbTransaction.Connection;
        }

        public virtual IEnumerable<T> Get<T>()
            where T : BaseDataObject
        {
            var sql = $"SELECT * FROM {this._tableName} WHERE Deleted = 0;";

            return this.Query<T>(sql);
        }

        public virtual T GetById<T>(string Id)
            where T : BaseDataObject
        {
            var sql = $"SELECT * FROM {this._tableName} WHERE Id = @Id;";

            return this.QueryFirstOrDefault<T>(sql, new { Id });
        }

        public virtual T GetByRefCode<T>(int RefCode)
            where T : BaseDataObject
        {
            var sql = $"SELECT * FROM {this._tableName} WHERE RefCode = @RefCode;";

            return this.QueryFirstOrDefault<T>(sql, new { RefCode });
        }

        public virtual int GetTableLines()
        {
            return this.GetTableLines(this._tableName);
        }


        public virtual bool DeleteAll()
        {
            var sql = $"DELETE FROM {this._tableName};";

            var res = this._dbConnection.Execute(sql, transaction: this._dbTransaction);

            return res > 0;
        }

        public virtual bool BulkAdd<T>(IEnumerable<T> values)
            where T : BaseDataObject
        {
            var result = false;

            var data_table = values.ToDataTable(this._tableName);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this._dbConnection as SqlConnection, SqlBulkCopyOptions.KeepIdentity, externalTransaction: this._dbTransaction as SqlTransaction))
            {
                bulkCopy.DestinationTableName = this._tableName;

                bulkCopy.WriteToServer(data_table);

                result = true;
            }

            return result;
        }

        protected virtual int GetTableLines(string table_name)
        {
            var sql = $"SELECT dbo.fn_GetTableLines('{table_name}') AS Count";

            return this._dbConnection.QueryFirstOrDefault<int>(sql, new { table_name }, this._dbTransaction);
        }

        protected T QueryFirstOrDefault<T>(string query, object parms = null)
        {
            return this._dbConnection.QueryFirstOrDefault<T>(query, parms, this._dbTransaction);
        }

        protected IEnumerable<T> Query<T>(string query, object parms = null)
        {
            return this._dbConnection.Query<T>(query, parms, this._dbTransaction);
        }

        #endregion  
    }
}
