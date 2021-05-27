using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace Entities
{
    public class DbClient
    {
        SQLiteConnection con;
        string connectionString;

        public DbClient(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Connect()
        {
            con = new SQLiteConnection (connectionString);
            con.Open();
        }

        public void Disconnect()
        {
            con.Close();
        }

        public SQLiteDataReader Query(string query)
        {
            return (new SQLiteCommand(query, con)).ExecuteReader();
        }

        public int Execute(string query)
        {
            return (new SQLiteCommand(query, con)).ExecuteNonQuery();
        }

        public List<string> GetTableNames()
        {
            List<string> result = new List<string>();
            foreach(DbDataRecord row in Query("select name from sqlite_master where type='table' order by 1;"))
                result.Add(row.GetString(0));
            return result;
        }
    }
}
