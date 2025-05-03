using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using DisconnectedMode;
using System.Data;



namespace DataSetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CacheDataSet cacheDataSet;
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
           cacheDataSet = new CacheDataSet();
           cacheDataSet.AddTable("Directions", "direction_id,direction_name");
           cacheDataSet.AddTable("Groups", "group_id,group_name,direction");
           cacheDataSet.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
           cacheDataSet.Print("Groups");
           cacheDataSet.Print("Directions");
           cbDirection.ItemsSource = cacheDataSet.Set.Tables["Directions"].DefaultView;
           cbDirection.DisplayMemberPath = "direction_name";
           cbDirection.SelectedValuePath = "direction_id";
            cbGroup.ItemsSource = cacheDataSet.Set.Tables["Groups"].DefaultView;
            cbGroup.DisplayMemberPath = "group_name";
            cbGroup.SelectedValuePath = "group_id";
            dgvGroup.ItemsSource = cbGroup.ItemsSource;

        }
        //private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dataTable = (((sender as ComboBox).ItemsSource) as DataTable);
        //    if (dataTable.ChildRelations.Count > 0)
        //        foreach (DataRelation dataRelation in dataTable.ChildRelations)
        //        {
        //            DataTable table =
        //            // ((this.Find($"cb{dataRelation.ChildTable.TableName.Remove(dataRelation.ChildTable.TableName.Length - 1)}", false)[0] as ComboBox).DataSource) as DataTable;
        //             ((this.FindName($"cb{dataRelation.ChildTable.TableName.Remove(dataRelation.ChildTable.TableName.Length - 1)}") as ComboBox).ItemsSource) as DataTable;
        //            table.DefaultView.RowFilter =
        //            $"{(sender as ComboBox).Name.Substring(Array.FindLastIndex<char>((sender as ComboBox).Name.ToCharArray(), Char.IsUpper)).ToLower()}" +
        //            $"={(sender as ComboBox).SelectedValue}";
        //        }
        //}
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        //https://ru.stackoverflow.com/questions/573008/wpf-datagrid-%D0%92%D1%8B%D1%80%D0%B0%D0%B2%D0%BD%D0%B8%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-%D0%B7%D0%B0%D0%B3%D0%BE%D0%BB%D0%BE%D0%B2%D0%BA%D0%B0-%D0%BA%D0%BE%D0%BB%D0%BE%D0%BD%D0%BA%D0%B8
        //https://ru.stackoverflow.com/questions/1258760/%D0%A6%D0%B5%D0%BD%D1%82%D1%80%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-%D1%82%D0%B5%D0%BA%D1%81%D1%82%D0%B0-%D0%BF%D0%BE-%D0%B3%D0%BE%D1%80%D0%B8%D0%B7%D0%BE%D0%BD%D1%82%D0%B0%D0%BB%D0%B8-%D0%B2-%D1%81%D1%82%D0%BE%D0%BB%D0%B1%D1%86%D0%B5-datagrid-%D0%B2-%D0%BF%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D0%B8-wpf
        //https://ru.stackoverflow.com/questions/723351/%D0%98%D0%B7%D0%BC%D0%B5%D0%BD%D0%B5%D0%BD%D0%B8%D0%B5-%D1%81%D1%82%D0%B8%D0%BB%D1%8F-%D0%BE%D0%BA%D0%BD%D0%B0-wpf
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           DataTable dataTable = (((sender as ComboBox).ItemsSource) as DataView).Table;
           if (dataTable.ChildRelations.Count > 0)
                foreach (DataRelation dataRelation in dataTable.ChildRelations)
                {
                    DataTable table =
                        (((this.FindName($"cb{dataRelation.ChildTable.TableName.Remove(dataRelation.ChildTable.TableName.Length - 1)}") as ComboBox).ItemsSource) as DataView).Table;
                    table.DefaultView.RowFilter =
                     $"{(sender as ComboBox).Name.Substring(Array.FindLastIndex<char>((sender as ComboBox).Name.ToCharArray(), Char.IsUpper)).ToLower()}" +
                     $"={(sender as ComboBox).SelectedValue}";
                }
            
        }
    }
}
