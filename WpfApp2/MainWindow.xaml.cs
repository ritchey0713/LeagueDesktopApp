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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp2
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    SqlConnection sQLConnection;
    public MainWindow()
    {
      InitializeComponent();

      string connectionString = ConfigurationManager.ConnectionStrings["WpfApp2.Properties.Settings.LeagueWPFConnectionString"].ConnectionString;

      sQLConnection = new SqlConnection(connectionString);
      ShowTeams();
    }

    private void ShowTeams()
    {

      try
      {
        string query = "select * from Member";
      
        // interface like data to make tables usable by c# objs
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sQLConnection);

        using (sqlDataAdapter)
        {
          DataTable memberTable = new DataTable();

          sqlDataAdapter.Fill(memberTable);

          // only displays one piece of data 
          ListTeams.DisplayMemberPath = "Name";

          // data to get selected item by
          ListTeams.SelectedValuePath = "Id";
          ListTeams.ItemsSource = memberTable.DefaultView;
        }

      } catch (Exception e)
      {
        MessageBox.Show(e.ToString());
      }

    }
  }
}
