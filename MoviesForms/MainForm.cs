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
    public partial class MainForm : Form
    {
        Connector connector;
        public MainForm()
        {
            InitializeComponent();
        }
        void data_initialization(string base_string)
        {
            Connector.@base = base_string;
            connector = new Connector();
            DataSet dataSet = connector.Select("TABLE_NAME", "INFORMATION_SCHEMA.TABLES");
            DataTable dt = dataSet.Tables[0];
            TreeNode[] treeNode = new TreeNode[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                treeNode[i] = new TreeNode(dt.Rows[i].ItemArray[0].ToString());
                foreach (DataTable dataTable in connector.Select("*", treeNode[i].Text).Tables)
                {
                    foreach (DataColumn dataColumn in dataTable.Columns) treeNode[i].Nodes.Add(new TreeNode(dataColumn.ColumnName));
                }
                treeView1.Nodes.Add(treeNode[i]);
            }
        }
        string string_query(ListBox listBox)
        {
            string query = "";
            for (int i = 0; i < listBox.Items.Count; i++)
            { 
                if(i>0) query += $", {listBox.Items[i].ToString()}";
                else query += $"{listBox.Items[i].ToString()}";
            } 
            return query;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode!=null)
            {
                foreach (TreeNode treeNode in treeView1.Nodes)
                {
                    if (treeNode.Text == treeView1.SelectedNode.Text)
                    {
                       if (listBoxTable.FindString(treeView1.SelectedNode.Text)<0) listBoxTable.Items.Add(treeView1.SelectedNode.Text);
                       treeView1.SelectedNode = null;
                       return;
                    }
                }
                if (listBoxColumn.FindString(treeView1.SelectedNode.Text)<0) listBoxColumn.Items.Add(treeView1.SelectedNode.Text);
                treeView1.SelectedNode = null;
                return;
            }
            if (textBoxComposite.Text!="")
            if(listBoxColumn.FindString(textBoxComposite.Text) < 0) listBoxColumn.Items.Add(textBoxComposite.Text);
        }

        private void listBoxColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxTable.SelectedItem = null;
        }

        private void listBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxColumn.SelectedItem = null;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxColumn.SelectedItem != null) listBoxColumn.Items.Remove(listBoxColumn.SelectedItem);
            if (listBoxTable.SelectedItem != null) listBoxTable.Items.Remove(listBoxTable.SelectedItem);
        }
      
        private void button1_Click(object sender, EventArgs e)
        {

            string query_column = string_query(listBoxColumn);
            string query_table = string_query(listBoxTable);
            if (query_column == "") query_column = "*";
            if (query_table == "") return;
            string query = $"SELECT {query_column} FROM {query_table}";
            if (textBoxCondition.Text != "") query += $" WHERE {textBoxCondition.Text}";
            SelectForm selectForm = new SelectForm();
            selectForm.textBox.Text = query;
            selectForm.Show();

        }

        private void comboBoxBases_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            data_initialization(comboBoxBases.SelectedItem.ToString());
        }

        private void buttonTableChange_Click(object sender, EventArgs e)
        {
            string query_table = string_query(listBoxTable);
            if (query_table == "") return;
            string query = $"SELECT * FROM {query_table}";
            TableChangeForm tableChangeForm = new TableChangeForm(query);
            tableChangeForm. Show();
        }
    }
}
