using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MoviesForms
{
    class Connector
    {
        static public string @base = "";
        readonly string CONNECTION_STRING;
        readonly SqlConnection connection;
        public Connector() : this(ConfigurationManager.ConnectionStrings[@base].ConnectionString) { }
       // public Connector() : this(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString) { }
        public Connector (string connection_string)
        {
            this.CONNECTION_STRING = connection_string;
            this.connection = new SqlConnection(CONNECTION_STRING);
        }
        public DataSet Select(string filds,string tables,string conditions = "")
        {
            string select = $"SELECT {filds} FROM {tables}";
            if (conditions != "") select += $"WHERE {conditions}";
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(select, connection);
            connection.Close();
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            return dataSet;
        }
        public DataSet Select(string select)
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(select, connection);
            connection.Close();
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            return dataSet;
        }
        public DataSet Insert(DataSet dataSet, string select)
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(select, connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Update(dataSet);
            connection.Close();
            dataSet.Clear();
            adapter.Fill(dataSet);
            return dataSet;
        }
    }
}
