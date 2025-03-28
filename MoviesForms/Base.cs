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
    public partial class Base : Form
    {
        public DataGridView dataGridView { get => dataGridView1; set => dataGridView1.DataSource = value; }
        public Base()
        {
            InitializeComponent();
          // Connector connector = new Connector();
           //dataGridView1.DataSource = connector.Select().Tables[0];

        }
    }
}
