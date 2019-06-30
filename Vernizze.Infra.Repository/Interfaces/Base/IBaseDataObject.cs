using System.Data;

namespace Vernizze.Infra.Repository.Interfaces.Base
{
    public interface IBaseDataObject
    {
        DataRow ToDataRow(DataTable data_table);
    }
}
