using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AllSmoInterFace
{
    public interface IAllSmo
    {

        List<object> DataBaseList(string ConnectionStringName);

        List<object> SchemaList(string ConnectionStringName, string DataBaseName);

        List<object> AllDiferentTable(List<string> listString, string DatabaseName);

        string DatabaseConturol(string ConnectionString, string DatabaseName);

        void CreateDatabaseSmo(string ConnectionStringName, string DataBaseName);

        List<object> StoredProceduresAllSmo(List<string> ConnectionStringName, string DatabaseName);

        List<object> ViewsAllSmo(List<string> ConnectionStringName, string DatabaseName);

    }
}
