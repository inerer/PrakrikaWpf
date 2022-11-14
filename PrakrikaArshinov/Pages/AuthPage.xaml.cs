using PrakrikaArshinov.U;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
using PrakrikaArshinov.Models;

namespace PrakrikaArshinov.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable UsersTable;
        DataTable ID;
        public AuthPage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
            UserRepository userRepository = new UserRepository(connectionString);
        }
        private void login_click(object sender, RoutedEventArgs e)
        {
           
        }   
        private void registration_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrPage());
        }
        private void AuthLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = TBLog.Text;
                string password = GetHash.GetHash1(TBPas.Password);
                UsersTable = new DataTable();
                ID = new DataTable();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    PartyMembers partyMembers = new PartyMembers();
                    connection.Open();
                    adapter = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand("SELECT* FROM [Party_members] WHERE [login] = @login and [password] = @password", connection);
                    command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
                    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                    adapter.SelectCommand = command;
                    adapter.Fill(UsersTable);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        partyMembers.IdMembers = reader.GetInt32(reader.GetOrdinal("id_member"));
                        partyMembers.Name = reader.GetString(reader.GetOrdinal("name"));
                        partyMembers.LastName = reader.GetString(reader.GetOrdinal("last_name"));
                        partyMembers.IdRank = reader.GetInt32(reader.GetOrdinal("id_rank"));
                        partyMembers.Photo = reader.GetString(reader.GetOrdinal("photo"));
                    }
                    if (UsersTable.Rows.Count > 0)
                    {
                        MessageBox.Show("Успешно авторизован");
                        NavigationService.Navigate(new MainPage(partyMembers));
                       
                    }
                       
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        TBPas.Clear();
                    }
                }  
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }  
    }
}
