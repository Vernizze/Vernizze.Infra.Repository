using Vernizze.Infra.CrossCutting.Extensions;
using Newtonsoft.Json;
using System;
using System.Data;
using Vernizze.Infra.Repository.VOs;
using Vernizze.Infra.Repository.Interfaces.Base;

namespace Vernizze.Infra.Repository.Abstract
{
    [Serializable]
    public abstract class BaseDataObject
        : IBaseDataObject
    {
        #region Constructors

        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Attributes

        public string Id { get; set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }

        [JsonIgnore]
        public bool Processed { get; set; }

        [JsonIgnore]
        public DateTimeOffset? ProcessedAt { get; set; }

        #endregion

        #region Methods

        public virtual DataRow ToDataRow(DataTable data_table)
        {
            var row = data_table.NewRow();

            row[nameof(this.Id)] = this.Id;

            if (this.CreatedAt.IsNull())
                row[nameof(this.CreatedAt)] = DBNull.Value;
            else
                row[nameof(this.CreatedAt)] = this.CreatedAt;

            if (this.UpdatedAt.IsNull())
                row[nameof(this.UpdatedAt)] = DBNull.Value;
            else
                row[nameof(this.UpdatedAt)] = this.UpdatedAt;

            row[nameof(this.Deleted)] = this.Deleted;

            row[nameof(this.Processed)] = this.Processed;

            if (this.ProcessedAt.IsNull())
                row[nameof(this.ProcessedAt)] = DBNull.Value;
            else
                row[nameof(this.ProcessedAt)] = this.ProcessedAt;

            return row;
        }

        public virtual DataRowEspecification GetDataRow()
        {
            var result = new DataRowEspecification();

            result.Especifications.Add(nameof(this.Id), typeof(string));
            result.Especifications.Add(nameof(this.CreatedAt), typeof(DateTimeOffset));
            result.Especifications.Add(nameof(this.UpdatedAt), typeof(DateTimeOffset));
            result.Especifications.Add(nameof(this.Deleted), typeof(bool));
            result.Especifications.Add(nameof(this.Processed), typeof(bool));
            result.Especifications.Add(nameof(this.ProcessedAt), typeof(DateTimeOffset));

            return result;
        }

        #endregion
    }
}
