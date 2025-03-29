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
    public partial class TableChangeForm : Form
    {   
        Connector connector;
        DataSet dataSet;
        string select { get; set; }
        public TableChangeForm(string query)
        {
            InitializeComponent();
            connector = new Connector();
            select = query;
            dataSet = connector.Select(query);
            dataGridViewChange.DataSource = dataSet.Tables[0];

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewChange.SelectedRows) dataGridViewChange.Rows.Remove(dataGridViewRow);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataRow dataRow = dataSet.Tables[0].NewRow();
            dataSet.Tables[0].Rows.Add(dataRow);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            dataSet = connector.Insert(dataSet, select);
        }
    }
}
