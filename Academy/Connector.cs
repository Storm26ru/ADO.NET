using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Academy
{
    class Connector
    {
        readonly string CONNECTION_STRING;
        SqlConnection connection;
        public Connector(string connection_string)
        {
            CONNECTION_STRING = connection_string;
            connection = new SqlConnection(CONNECTION_STRING);
            AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
        }
        public DataTable Select(string columns, string tables, string condition ="",string group_by = "")
        {
            DataTable table = null;
            string cmd = $"SELECT {columns} FROM {tables}";
            if (condition != "") cmd += $" WHERE {condition}";
            if (group_by != "") cmd += $" GROUP BY {group_by}";
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                table = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++) table.Columns.Add(reader.GetName(i));
                while (reader.Read())
                {
                    DataRow row = table.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++) row[i] = reader[i];
                    table.Rows.Add(row);
                }
            }
            reader.Close();
            connection.Close();
            return table;
        }
        public void Insert(DataTable dataTable,string table,string test_condition)
        {
            string condition = "";
            string columns = "";
            string values = "";
            foreach (DataColumn column in dataTable.Columns)
            { 
                columns += column.Ordinal != 0 ? $", {column.ColumnName}" : $"{column.ColumnName}";
                if (column.ColumnName == test_condition) condition = $"{test_condition} = '{dataTable.Rows[0].ItemArray[column.Ordinal].ToString()}'";
            }        
            for(int i =0; i<dataTable.Columns.Count; i++)
            {
                values += i != 0 ? $", '{dataTable.Rows[0].ItemArray[i].ToString()}'" : $"'{dataTable.Rows[0].ItemArray[i].ToString()}'";
            }
            string verification = $"IF NOT EXISTS (SELECT * FROM {table} WHERE {condition})";
            string query = $"INSERT {table} ({columns}) VALUES ({values})";
            string cmd = $"{verification} BEGIN {query} END";
            Console.WriteLine(cmd);
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

    }
}
