using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Data.SqlClient;
using System.Configuration;
using DisconnectedMode;
//using System.Threading;



namespace AcademyDataSet
{
    public partial class MainForm : Form
    {
        CacheDataSet cacheDataSet;
        public MainForm()
        {
            InitializeComponent();
            AllocConsole();
            cacheDataSet = new CacheDataSet();
            cacheDataSet.AddTable("Directions", "direction_id,direction_name");
            cacheDataSet.AddTable("Groups", "group_id,group_name,direction");
            cacheDataSet.AddRelation("GroupsDirections","Groups,direction", "Directions,direction_id");
            cacheDataSet.Print("Groups");
            cacheDataSet.Print("Directions");
            // cbDirection.DataSource = cacheDataSet.GetDataTable("Directions");
            cbDirection.DataSource = cacheDataSet.Set.Tables["Directions"];
            cbDirection.DisplayMember = "direction_name";
            cbDirection.ValueMember = "direction_id";
            //cbGroup.DataSource = cacheDataSet.GetDataTable("Groups");
            cbGroup.DataSource = cacheDataSet.Set.Tables["Groups"];
            cbGroup.DisplayMember = "group_name";
            cbGroup.ValueMember = "group_id";
            dgvGroup.DataSource = cbGroup.DataSource;

        }
        //public void UpdateData(object obj)
        //{
        //   // DataTable table = cbGroup.DataSource as DataTable;
        //   // table.Clear();
        //    cacheDataSet.UpdateDataSet();
        //    //cbDirection.DataSource = null;
        //    // cbDirection.DataSource = cacheDataSet.GetDataTable("Directions");
        //    // cbGroup.DataSource = null;
        //    // cbGroup.DataSource = cacheDataSet.GetDataTable("Groups");
        //    // dgvGroup.DataSource = null;
        //    //dgvGroup.DataSource = cbGroup.DataSource;
        //    cacheDataSet.Print("Groups");
        //}
        
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dataTable = (((sender as ComboBox).DataSource) as DataTable);
            if(dataTable.ChildRelations.Count>0)
                foreach(DataRelation dataRelation in dataTable.ChildRelations)
                {
                    DataTable table =
                     ((this.Controls.Find($"cb{dataRelation.ChildTable.TableName.Remove(dataRelation.ChildTable.TableName.Length - 1)}", false)[0] as ComboBox).DataSource) as DataTable;
                    table.DefaultView.RowFilter =
                    $"{(sender as ComboBox).Name.Substring(Array.FindLastIndex<char>((sender as ComboBox).Name.ToCharArray(), Char.IsUpper)).ToLower()}" +
                    $"={(sender as ComboBox).SelectedValue}";
                }
        }
    }
}
