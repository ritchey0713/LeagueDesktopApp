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
      ShowAllMembers();
    }

    private void ShowTeams()
    {

      try
      {
        string query = "select * from LeagueTeam";
      
        // interface like data to make tables usable by c# objs
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sQLConnection);

        using (sqlDataAdapter)
        {
          DataTable TeamTable = new DataTable();

          sqlDataAdapter.Fill(TeamTable);

          // only displays one piece of data 
          ListTeams.DisplayMemberPath = "Name";

          // data to get selected item by
          ListTeams.SelectedValuePath = "Id";
          ListTeams.ItemsSource = TeamTable.DefaultView;
        }

      } catch (Exception e)
      {
        MessageBox.Show(e.ToString());
      }

    }

    private void ShowAllMembers()
    {
      try
      {
        string query = "select * from Member";

        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sQLConnection);

        using (sqlDataAdapter)
        {
          DataTable MembersTable = new DataTable();
          sqlDataAdapter.Fill(MembersTable);

          AllMembers.DisplayMemberPath = "Name";
          AllMembers.SelectedValuePath = "Id";
          AllMembers.ItemsSource = MembersTable.DefaultView;
        }
      } catch(Exception e)
      {
        MessageBox.Show(e.ToString());
      }
    }

    private void ShowLeagueTeamMembers()
    {

      try
      {
        string query = "select * from Member m inner join LeagueTeamMember ltm on m.Id = ltm.memberId where ltm.LeagueTeamId = @LeagueTeamId";

        SqlCommand sqlCommand = new SqlCommand(query, sQLConnection);

        // interface like data to make tables usable by c# objs
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

        using (sqlDataAdapter)
        {

          sqlCommand.Parameters.AddWithValue("@LeagueTeamId", ListTeams.SelectedValue);

          DataTable TeamMemberTable = new DataTable();

          sqlDataAdapter.Fill(TeamMemberTable);

          // only displays one piece of data 
          LeagueTeamMembers.DisplayMemberPath = "Name";

          // data to get selected item by
          LeagueTeamMembers.SelectedValuePath = "Id";
          LeagueTeamMembers.ItemsSource = TeamMemberTable.DefaultView;
        }

      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString());
      }

    }

    private void ListTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //MessageBox.Show(ListTeams.SelectedValue.ToString());
      if(ListTeams.SelectedItem != null)
      {
        ShowLeagueTeamMembers();
      }
    }

    private void DeleteTeam_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string query = "delete from LeagueTeam where id = @LeagueTeamId";
        SqlCommand sqlCommand = new SqlCommand(query, sQLConnection);
        sQLConnection.Open();
        sqlCommand.Parameters.AddWithValue("@LeagueTeamId", ListTeams.SelectedValue);
        // simple way to execute simple sql queries
        sqlCommand.ExecuteScalar();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
      finally
      {
        sQLConnection.Close();
        ShowTeams();
      }
    }

    private void AddTeam_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string query = "insert into LeagueTeam values (@Name)";
        SqlCommand sqlCommand = new SqlCommand(query, sQLConnection);
        sQLConnection.Open();
        sqlCommand.Parameters.AddWithValue("@Name", CreateItem.Text);
        // simple way to execute simple sql queries
        sqlCommand.ExecuteScalar();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
      finally
      {
        sQLConnection.Close();
        CreateItem.Text = "";
        ShowTeams();
      }
    }

  }// end MainWindow
}
