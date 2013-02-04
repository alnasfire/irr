using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Data;

namespace irrparser.db
{
    class AdvertDAO
    {
        private SqlConnection myConnection = null;
        public AdvertDAO()
        {
            this.myConnection = new Connector().GetOpenConnection();
        }

        public SqlConnection GetConnection()
        {
            return myConnection;
        }

        public void AddAdvert(Advert advert)
        {
            int ag = advert.IsAgent() ? 1 : 0;
            List<String> errors = new List<String>();
            SqlCommand command = myConnection.CreateCommand();
            command.CommandText = "INSERT INTO [nasgor].[dbo].[Adverts] VALUES ('" + advert.getHeader() + "', '" + advert.getPhone() + "', '" + advert.getPrice() + "', '" + ag + "');";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                File.WriteAllLines("C://Users/nasgor/My Documents/clear.txt", errors);
            }
        }

        public List<Advert> GetAllAdverts()
        {
            List<Advert> adverts = new List<Advert>();
            DataTable data = new DataTable();
            SqlCommand command = new SqlCommand("Select * From [nasgor].[dbo].[Adverts]");
            command.Connection = myConnection;
            try
            {
                data.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            foreach(DataRow row in data.Rows)
            {
                Boolean ag = row["agent"].ToString().Equals("1") ? true : false;
                adverts.Add(new Advert(row["advert"].ToString(), row["phone"].ToString(), row["price"].ToString(), ag));    
            }
            return adverts;
        }
    }
}
