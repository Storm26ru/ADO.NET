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
       
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();


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
