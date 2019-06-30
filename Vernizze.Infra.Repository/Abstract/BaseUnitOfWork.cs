using Vernizze.Infra.CrossCutting.DataObjects.AppSettings;
using Vernizze.Infra.CrossCutting.Extensions;
using Vernizze.Infra.Repository.Interfaces.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Vernizze.Infra.Repository.Abstract
{
    public abstract class BaseUnitOfWork
        : IUnitOfWork
    {
        #region Variables

        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        protected Dictionary<string, IBaseRepository> _repositories = new Dictionary<string, IBaseRepository>();

        #endregion

        #region Attributes

        public IDbConnection Connection { get { return this._connection; } }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (!this._transaction.IsNull())
                this._transaction.Rollback();

            if (!this._connection.IsNull())
                this._connection.Close();
        }

        protected ConnectionStrings CryptConfig(ConnectionStringsFilePath dbSettings)
        {
            var asm = Assembly.GetExecutingAssembly();
            var key = asm.GetType().GUID.ToString().Replace("-", string.Empty);
            var res = false;
            var result = new ConnectionStrings();

            var connection_string_path = dbSettings.connection_string_path;
            var connections_strings_json = System.IO.File.ReadAllText($@"{connection_string_path}connections_strings.json");

            var connection_strings = JsonConvert.DeserializeObject<ConnectionStrings>(connections_strings_json);

            var is_protected = res.Parse2(connection_strings.is_protected);

            if (!is_protected)
                connection_strings.Protect(key);

            result = connection_strings.ReadProtected(key);

            return result;
        }

        public abstract void InitReps(IDbConnection sqlConnection);

        protected abstract void InitReps(IDbTransaction sqlConnection);

        public IBaseRepository GetRepository(string table_name)
        {
            this._repositories.TryGetValue(table_name, out IBaseRepository result);

            return result;
        }

        public void OpenConnection()
        {
            this._connection.Open();

            this.InitReps(this._connection);
        }

        public void Begin()
        {
            this._transaction = this._connection.BeginTransaction();

            this.InitReps(this._transaction);
        }

        public void Commit()
        {
            try
            {
                if (this._connection.State.Equals(ConnectionState.Open))
                {
                    try
                    {
                        this._transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        this._transaction.Rollback();

                        Trace.WriteLine(ex);

                        throw;
                    }
                    finally
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                }
                else
                {
                    if (!this._transaction.IsNull())
                        this._transaction.Dispose();

                    this._transaction = null;
                }
            }
            catch (Exception ex)
            {
                if (!this._transaction.IsNull())
                    this._transaction.Dispose();

                this._transaction = null;

                Trace.WriteLine(ex);

                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                if (this._connection.State.Equals(ConnectionState.Open))
                {
                    try
                    {
                        this._transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        this._transaction.Rollback();

                        Trace.WriteLine(ex);

                        throw;
                    }
                    finally
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                }
                else
                {
                    if (!this._transaction.IsNull())
                        this._transaction.Dispose();

                    this._transaction = null;
                }
            }
            catch (Exception ex)
            {
                if (!this._transaction.IsNull())
                    this._transaction.Dispose();

                this._transaction = null;

                Trace.WriteLine(ex);

                throw;
            }
        }

        #endregion
    }
}
