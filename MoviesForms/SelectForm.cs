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
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
           Connector connector = new Connector();
           Base @base = new Base();
           @base.dataGridView.DataSource = connector.Select(textBoxQuery.Text).Tables[0];
           @base.Show();
        }
    }
}
