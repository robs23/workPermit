using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workPermit
{
    public static class Variables
    {
        public static string npdConnectionString = Static.Secrets.ConnectionString;
        public static SqlConnection connx = new SqlConnection(Static.Secrets.ConnectionString);

    }
}
