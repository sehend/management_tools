using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PropertyModel
{
    public class CreateScripAllModel
    {
        public string? BaseEnvarimantName { get; set; }

        public string? DatabaseName { get; set; }

        public List<String>? TableName { get; set; }

        public List<String>? ColLumnsName { get; set; }

        public List<String>? IndexName { get; set; }

        public List<String>? Views { get; set; }

        public List<String>? StoredProcedures { get; set; }

        public string? NickName { get; set; }

        public string? Password { get; set; }


    }
}
