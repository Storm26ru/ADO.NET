using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Academy
{
    public partial class FormEditTable : Form
    {
        Connector connector;
        DataTable dataTable;
        string table { get; set; }
        public FormEditTable(string table)
        {
            this.table = table;
            InitializeComponent();
            connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            dataTable = connector.Select("TOP 1 *", table);
            dataTable.Rows[0].Delete();
            dataTable.NewRow();
            dgvAdd.DataSource = dataTable;
            //dataTable.AcceptChanges();
            MainForm main = new MainForm();
            //main.D_directions.Add("ht", 2);
           //Console.WriteLine(main.D_directions["ht"]);
            //Console.WriteLine(main.d_directions["Разработка программного обеспечения"]);

            

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            connector.Insert(dataTable, table, "group_name");
        }
    }
}
