using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace AcademyDataSet
{
    class Cache
    {
        readonly string CONNECTION_STRING = "";
        SqlConnection connection = null;
        DataSet set = null;
        public Cache() : this(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString) { }
        public Cache(string connection_strin)
        {
            this.CONNECTION_STRING = connection_strin;
            this.connection = new SqlConnection(CONNECTION_STRING);
            this.set = new DataSet();
        }
        public void AddTable(string table, string columns)
        {
            set.Tables.Add(table);
            string[] a_columns = columns.Split(',');
            for (int i = 0; i < a_columns.Length; i++)
            {
                set.Tables[table].Columns.Add(a_columns[i]);
            }
            set.Tables[table].PrimaryKey =
                new DataColumn[] { set.Tables[table].Columns[0] };
            string cmd = $"SELECT {columns} FROM {table}";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
            adapter.Fill(set.Tables[table]);
            //Print(table);
        }
        public void AddRelation(string relation_name, string child, string parent)
        {
            set.Relations.Add
                (
                    relation_name,
                    set.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
                    set.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
                );
        }
        public DataTable GetDataTable(string table) => set.Tables[table];
        public void Print(string table)
        {
            Console.WriteLine(table);
            Console.WriteLine("\n================================================================\n");
            foreach (DataColumn column in set.Tables[table].Columns)
                Console.Write(column.ColumnName + "\t");
            Console.WriteLine("\n----------------------------------------------------------------\n");
            for (int i = 0; i < set.Tables[table].Rows.Count; i++)
            {
                for (int j = 0; j < set.Tables[table].Columns.Count; j++)
                {
                    if (set.Tables[table].ParentRelations.Count > 0)
                    {
                        foreach (DataRelation relation in set.Tables[table].ParentRelations)
                            if (relation.ChildColumns.Any(c => c.ColumnName == set.Tables[table].Columns[j].ColumnName))
                                Console.Write(set.Tables[table].Rows[i].GetParentRow(relation)[$"{set.Tables[table].Columns[j].ColumnName}_name"] + "\t");
                            else Console.Write(set.Tables[table].Rows[i][j] + "\t\t");
                    }
                    else Console.Write(set.Tables[table].Rows[i][j] + "\t\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n================================================================\n");

        }
    }
}
