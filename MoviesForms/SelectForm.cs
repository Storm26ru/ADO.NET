using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoviesForms
{
    public partial class SelectForm : Form
    {
        public TextBox textBox { get => textBoxQuery; set => textBoxQuery.Text = value.Text; }
        public SelectForm()
        {
            InitializeComponent();

            //foreach (DataTable dt in connector.Select().Tables)
            //{
            //    foreach (DataRow dr in dt.Rows) listBoxSelect.Items.AddRange(dr.ItemArray);
            //}
                
            
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
           Connector connector = new Connector();
           Base @base = new Base();
           @base.dataGridView.DataSource = connector.Select(textBoxQuery.Text).Tables[0];
          //@base.dataGridView.DataSource = connector.Select("TABLE_NAME", "INFORMATION_SCHEMA.TABLES").Tables[0];
           @base.Show();
        }

        //private void buttonAdd_Click(object sender, EventArgs e)
        //{
        //    table = listBoxSelect.SelectedItem.ToString(); 
        //}
    }
}
