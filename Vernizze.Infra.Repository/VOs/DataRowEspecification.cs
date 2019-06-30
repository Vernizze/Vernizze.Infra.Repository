using System.Collections.Generic;

namespace Vernizze.Infra.Repository.VOs
{
    public class DataRowEspecification
    {
        private Dictionary<string, System.Type> _data = new Dictionary<string, System.Type>();

        public Dictionary<string, System.Type> Especifications
        {
            get { return this._data; }
            set { this._data = value; }
        }
    }
}
