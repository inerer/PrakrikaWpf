using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using PrakrikaArshinov.U;
using System.Configuration;
using PrakrikaArshinov.Models;

namespace PrakrikaArshinov.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrPage.xaml
    /// </summary>
    public partial class RegistrPage : Page
    {
        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
        string connectionString;
        SqlDataAdapter adapter;
        List<PartyMembers> partyMembers;
       
        public RegistrPage()
        {
            try
            {
                InitializeComponent();
                connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
                UserRepository userRepository = new UserRepository(connectionString);
                partyMembers = new List<PartyMembers>();
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
            
        }
        PartyMembers member;
        private bool PasswordOkey()
        {
            return PasswordTextBox.Password.Length >= 8 &&
                    PasswordTextBox.Password.Any(Char.IsDigit) &&
                    PasswordTextBox.Password.Any(Char.IsLetter) &&
                    PasswordTextBox.Password.Any(Char.IsPunctuation) &&
                    PasswordTextBox.Password.Any(Char.IsUpper) &&
                    PasswordTextBox.Password.Any(Char.IsLower);
        }
        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PasswordOkey())
                {
                    member = partyMembers.Where(c => c.login.Contains(LoginTextBox.Text) || c.password.Contains(GetHash.GetHash1(PasswordTextBox.Password))).FirstOrDefault();
                    if (member == null)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            adapter = new SqlDataAdapter();
                            SqlCommand command = new SqlCommand("Insert into  [Party_members]  ([name], [last_name], [middle_name], [id_rank], [login], [password], [photo]) values (@1, @2, @3, @4, @5, @6, @7)", connection);
                            command.Parameters.Add("@1", SqlDbType.NVarChar).Value = FirstNameTextBox.Text;
                            command.Parameters.Add("@2", SqlDbType.NVarChar).Value = LastNameTextBox.Text;
                            command.Parameters.Add("@3", SqlDbType.NVarChar).Value = MiddleNameTextBox.Text;
                            command.Parameters.Add("@4", SqlDbType.Int).Value = (RoleBox.SelectedItem as RankParty).Id;
                            command.Parameters.Add("@5", SqlDbType.NVarChar).Value = LoginTextBox.Text;
                            command.Parameters.Add("@6", SqlDbType.NVarChar).Value = GetHash.GetHash1(PasswordTextBox.Password);
                            command.Parameters.Add("@7", SqlDbType.NVarChar).Value = openFileDialog.FileName;
                            command.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Данные были добавлены");
                        }
                    }
                    else
                        MessageBox.Show("Логин или пароль не уникальны!");
                    }
                    else
                        MessageBox.Show("Неподходящий пароль");
                }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }          
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<RankParty> listType = new List<RankParty>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmdt = new SqlCommand("Select * from [rank_party]", connection))
                    {
                        using (SqlDataReader reader = cmdt.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RankParty Type = new RankParty();
                                Type.Id = reader.GetInt32(0);
                                Type.Name = reader.GetString(1);
                                listType.Add(Type);
                            }
                        }
                    }
                    RoleBox.ItemsSource = listType;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmdt = new SqlCommand("Select * from Party_members", connection))
                    {
                        using (SqlDataReader reader = cmdt.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PartyMembers Type = new PartyMembers();
                                Type.login = reader.GetString(reader.GetOrdinal("login"));
                                Type.password = reader.GetString(reader.GetOrdinal("password"));
                            partyMembers.Add(Type);
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
}
        private void Click_Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                openFileDialog.FileName = "";
                openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
             "Portable Network Graphic (*.png)|*.png";
                Nullable<bool> result = openFileDialog.ShowDialog();
                if (result == true)
                    UserImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Border_Drop(object sender, DragEventArgs e)
        {

            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 1)
                    MessageBox.Show("Нужно выбрать 1 файл!");
                else
                {
                    string file = files[0];
                    if (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                    {
                        Image image = new Image();
                        image.Width = 150;
                        image.Height = 150;
                        image.Source = new BitmapImage(new Uri(file));
                        UserImage.Source = new BitmapImage(new Uri(file));
                        openFileDialog.FileName = files[0];
                    }
                    else
                    {
                        MessageBox.Show("Только картинки");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");

            }
        }
    }
}
