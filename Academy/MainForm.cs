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
using System.Reflection;
using System.Collections.ObjectModel;
using ConnectedMode;

namespace Academy
{
    public partial class MainForm : Form
    {
        FormEditTable formEdit;
        ConnectedMode.Connector connector;
        //Connector connector;
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

        Dictionary<string, int> d_directions;
        public ReadOnlyDictionary<string, int> rod_directions { get => new ReadOnlyDictionary<string, int>(d_directions); }
        Dictionary<string, int> d_groups;
        public ReadOnlyDictionary<string, int> rod_groups { get => new ReadOnlyDictionary<string, int>(d_groups); }
        Dictionary<ComboBox, ComboBox> cb_parent;
        
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
            connector = new ConnectedMode.Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            //connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            dgvStudents.DataSource = connector.Select("stud_id,last_name,first_name,middle_name,birth_date,group_name,direction_name",
                                                      "Students JOIN Groups ON [group]=group_id JOIN Directions ON direction=direction_id");
            statusStripCountLabel.Text = $"Колличество студетов: {dgvStudents.RowCount - 1}";
            d_directions = connector.GetDictionary("Directions");
            d_groups = connector.GetDictionary("Groups");
            cbGroupsDirections.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
            cbStudentsDirections.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
            cbStudentsGroups.Items.AddRange(d_groups.Select(d => d.Key).ToArray());
            cb_parent = new Dictionary<ComboBox,ComboBox>();
            cb_parent.Add(cbStudentsGroups, cbStudentsDirections);
          
            
        }
       
        void LoadTab(Query query = null)
        {
            int i = tabControl.SelectedIndex;
            if (query == null) query = queries[i];
            tables[i].DataSource = connector.Select(query.Column, query.Table, query.Condition, query.GroupBy);
            statusStripCountLabel.Text = $"{status_messages[i]}{tables[i].RowCount - 1}";

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTab();
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

        Query cbQuery(object sender,Query query)
        {
            string tab_name = (sender as ComboBox).Name;
            string fild_name = tab_name.Substring(Array.FindLastIndex<char>(tab_name.ToCharArray(), Char.IsUpper));//.ToLower();
            ReadOnlyDictionary<string, int> source = this.GetType().GetProperty($"rod_{fild_name.ToLower()}").GetValue(this) as ReadOnlyDictionary<string, int>;
            if (query.Condition != "") query.Condition += " AND";
            query.Condition += $"[{fild_name.Remove(fild_name.Length - 1).ToLower()}] = {source[(sender as ComboBox).SelectedItem.ToString()]}";
            if(cb_parent.ContainsValue(sender as ComboBox))
            {
                foreach(KeyValuePair<ComboBox,ComboBox> keyValuePair in cb_parent )
                {
                    if (keyValuePair.Value == (sender as ComboBox))
                    {
                        string tab_name_parent = keyValuePair.Key.Name;
                        string fild_name_parent = tab_name_parent.Substring(Array.FindLastIndex<char>(tab_name_parent.ToCharArray(), Char.IsUpper));
                        string condition = $"[{fild_name.Remove(fild_name.Length - 1).ToLower()}_id] = {source[(sender as ComboBox).SelectedItem.ToString()]}";
                        keyValuePair.Key.Items.Clear();
                        keyValuePair.Key.Items.AddRange(connector.GetDictionary(fild_name_parent, fild_name, condition)
                            .Select(d => d.Key).ToArray());
                    }
                }
            }
            if (cb_parent.ContainsKey(sender as ComboBox)&& cb_parent[sender as ComboBox].SelectedIndex>=0)
                query = cbQuery(cb_parent[sender as ComboBox],query);
            return query;
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query query = new Query(queries[tabControl.SelectedIndex]);
           query = cbQuery(sender, query);
            LoadTab(query);
        }
    }
}
