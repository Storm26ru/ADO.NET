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
    public partial class MainForm : Form
    {
        FormEditTable formEdit;
        Connector connector;
        Query[] queries = new Query[]
        {
            new Query("stud_id,last_name,first_name,middle_name,birth_date,group_name,direction_name",
                      "Students JOIN Groups ON [group]=group_id JOIN Directions ON direction=direction_id"),
            new Query("group_id,group_name,direction_name,COUNT(stud_id) AS N'Student population'","Students, Groups,Directions","direction = direction_id AND [group]=group_id",
                      "group_id,group_name,direction_name"),
            new Query("direction_name,COUNT(stud_id) AS N'Student population',COUNT (DISTINCT group_id) AS N'Number of groups'",
                       "Groups JOIN Students ON [group]= group_id JOIN Directions ON direction=direction_id","",
                        "direction_name"),
            new Query("*","Disciplines"),
            new Query("*","Teachers")
        };
        DataGridView[] tables;
        string[] status_messages = new string[]
        {
            "Кличество студентов: ",
            "Кличество групп: ",
            "Кличество направлений: ",
            "Кличество дисцилин: ",
            "Кличество преподавателей: ",
        };

        public MainForm()
        {
            InitializeComponent();
            tables = new DataGridView[]
            {
                dgvStudents,
                dgvGroups,
                dgvDirections,
                dgvDiscoplines,
                dgvTeachers
            };
            connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            dgvStudents.DataSource = connector.Select("stud_id,last_name,first_name,middle_name,birth_date,group_name,direction_name",
                                                      "Students JOIN Groups ON [group]=group_id JOIN Directions ON direction=direction_id");
            statusStripCountLabel.Text = $"Колличество студетов: {dgvStudents.RowCount - 1}";
            comboBoxDirection.Items.AddRange(AddItems("DISTINCT direction_name", "Groups,Directions", "direction=direction_id"));
            comboBoxDirection.Items.Insert(0, "All");
            cmbStudentsDirections.Items.AddRange(AddItems("DISTINCT direction_name", "Students,Groups,Directions",
                                                          "[group]=group_id AND direction=direction_id "));
            cmbStudentsDirections.Items.Insert(0, "All");
            cmbStudentsGroups.Items.AddRange(AddItems("DISTINCT group_name", "Students,Groups","[group]=group_id"));
            cmbStudentsGroups.Items.Insert(0, "All");
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (checkBoxGroups.Checked) return;
            int i = tabControl.SelectedIndex;
            tables[i].DataSource = connector.Select(queries[i].Column, queries[i].Table, queries[i].Condition, queries[i].GroupBy);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";
            //if (tabControl.SelectedTab.Text == "Groups")
            //{
            //    //for(int j = 0; j<tables[i].RowCount-1;j++)
            //    //{
            //    //    for(int k=0; k<tables[i].Columns.Count;k++)
            //    //    {
            //    //        if(tables[i].Rows[j].Cells[k].OwningColumn.Name == "direction_name")
            //    //        {
            //    //             if(comboBoxDirection.FindString(tables[i].Rows[j].Cells[k].Value.ToString())<0)
            //    //            comboBoxDirection.Items.Add(tables[i].Rows[j].Cells[k].Value);
            //    //        }
            //    //    }
            //    //}
            //    //DataTable dataTable = connector.Select("DISTINCT direction_name", "Groups,Directions", "direction=direction_id");
            //    //foreach (DataRow row in dataTable.Rows)
            //    //{
            //    //    foreach (object cell in row.ItemArray) comboBoxDirection.Items.Add(cell);
            //    //}
            //}
        }
        string[] AddItems(string colunmn,string table,string condition)
        {
            DataTable dataTable = connector.Select(colunmn, table, condition);
            string[] items = new string[dataTable.Rows.Count];
            for (int i = 0; i < items.Length; i++) foreach (object cell in dataTable.Rows[i].ItemArray) items[i] = cell.ToString();
            return items;
        }

        private void checkBoxGroups_CheckedChanged(object sender, EventArgs e)
        {
            string condition = "";
            int i = tabControl.SelectedIndex;
            if (comboBoxDirection.SelectedIndex>0) condition =$"direction_name='{comboBoxDirection.Text}'";
            if (checkBoxGroups.Checked)
                dgvGroups.DataSource = connector.Select("group_id, group_name, direction_name, COUNT(stud_id) AS N'Student population'",
                                                        "Groups LEFT JOIN Students ON[group] = group_id JOIN Directions ON direction = direction_id",
                                                        condition, "group_id, group_name, direction_name");
            else comboBoxDirection_SelectedIndexChanged(sender, e);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";
        }

        private void comboBoxDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            string condition = queries[i].Condition;
            if (comboBoxDirection.SelectedIndex > 0) condition = $"{queries[i].Condition} AND direction_name='{comboBoxDirection.Text}'";
            if (checkBoxGroups.Checked) checkBoxGroups_CheckedChanged(sender, e);
            else dgvGroups.DataSource = connector.Select(queries[i].Column, queries[i].Table,condition, queries[i].GroupBy);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";
        }

        private void cmbStudentsDirections_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            string condition ="";
            if (cmbStudentsDirections.SelectedIndex > 0) condition = $"direction_name='{cmbStudentsDirections.Text}'";
           // dgvStudents.DataSource = connector.Select(queries[i].Column,queries[i].Table,condition);
            cmbStudentsGroups.Items.Clear();
            cmbStudentsGroups.Items.AddRange(AddItems("DISTINCT group_name", queries[i].Table, condition));
            cmbStudentsGroups.Items.Insert(0, "All");
            cmbStudentsGroups.SelectedIndex=0;
        }

        private void cmbStudentsGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            string condition ="";
            if (cmbStudentsDirections.SelectedIndex > 0) condition = $"direction_name='{cmbStudentsDirections.Text}'";
            if (cmbStudentsGroups.SelectedIndex > 0) condition+= cmbStudentsDirections.SelectedIndex > 0 ? $" AND group_name = '{cmbStudentsGroups.Text}'" : 
                                                                                                           $"group_name = '{cmbStudentsGroups.Text}'";
            dgvStudents.DataSource = connector.Select(queries[i].Column, queries[i].Table, condition);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";
        }

        private void chbEmptyDirections_CheckedChanged(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            if (chbEmptyDirections.Checked)
                dgvDirections.DataSource = connector.Select(queries[i].Column, "Groups LEFT JOIN Students ON[group] = group_id RIGHT JOIN Directions ON direction = direction_id", "", queries[i].GroupBy);
            else tabControl_SelectedIndexChanged(sender, e);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            formEdit = new FormEditTable(tabControl.SelectedTab.Text.ToString());
            formEdit.Show();
        }
    }
}
