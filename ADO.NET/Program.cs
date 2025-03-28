using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            const int PADDING = 24;
            const string CONNECTION_STRING = @"
                         Data Source = (localdb)\MSSQLLocalDB; 
                         Initial Catalog = Movies_VPD_311; 
                         Integrated Security = True; 
                         Connect Timeout = 30; 
                         Encrypt = False; 
                         TrustServerCertificate = False; 
                         ApplicationIntent = ReadWrite; 
                         MultiSubnetFailover = False";
            SqlConnection connection = new SqlConnection(CONNECTION_STRING);
            string cmd = "SELECT * FROM Directors,Movies WHERE director_id = director";
            SqlCommand command = new SqlCommand(cmd,connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++) Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
                Console.WriteLine();

                while(reader.Read())
                {
                    //Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}");
                    for (int i = 0; i < reader.FieldCount; i++) Console.Write(reader[i].ToString().PadRight(PADDING));
                    Console.WriteLine();
                }
            }
            reader.Close();
            connection.Close();
        }
    }
}
