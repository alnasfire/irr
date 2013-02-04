using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irrparser.db
{
    class Connector
    {
        public Connector()
        {
        }

        public SqlConnection GetOpenConnection()
        {
            string sCxn = "Data Source=NASGOR-PC\\SQLEXPRESS;Initial Catalog=nasgor;Integrated Security=True";
            SqlConnection myConnection = null;
            try
            {
                myConnection = new SqlConnection(sCxn);
                myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return myConnection;
        }

        public void CloseConnection(SqlConnection myConnection)
        {
            myConnection.Close();
        }
    }
}
