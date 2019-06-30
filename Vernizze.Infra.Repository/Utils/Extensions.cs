using System.Collections.Generic;
using System.Data;
using System.Linq;

using Vernizze.Infra.CrossCutting.Extensions;
using Vernizze.Infra.Repository.Abstract;

namespace Vernizze.Infra.Repository.Utils
{
    public static class Extensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data, string table_name)
            where T : BaseDataObject
        {
            var result = new DataTable(table_name);

            if (data.HaveAny())
            {
                var type = typeof(T);

                var spec_row = data.First().GetDataRow().Especifications;

                foreach (KeyValuePair<string, System.Type> entry in spec_row)
                {
                    result.Columns.Add(entry.Key, entry.Value);
                }

                data.ToList().ForEach(r =>
                {
                    result.Rows.Add(r.ToDataRow(result));
                });
            }

            return result;
        }
    }
}
