using PrakrikaArshinov.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace PrakrikaArshinov.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        Result _result;
        string connectionString;
        SqlDataAdapter adapter;
        List<Result> results = new List<Result>();
        public AddPage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
            EditButton.Visibility = Visibility.Collapsed;
            
        }
        public AddPage(Result result)
        {
            InitializeComponent();
            var selectedItem = result;
            this.DataContext =selectedItem;
            _result = result;
            connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
            CityNameTextBox.Text = selectedItem.CityName;
            EventNameTextBox.Text = selectedItem.EventName;
            DatePicker.Text =Convert.ToString(selectedItem.date);
            AddEditButton.Visibility = Visibility.Collapsed;
        }
        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            CityList cityList = new CityList();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Insert into [City_list] (city_name) output inserted.id_city values (@1)", connection);
                command.Parameters.Add("@1", SqlDbType.NVarChar).Value = CityNameTextBox.Text;
                SqlDataReader reader3 = command.ExecuteReader();
                while(reader3.Read())
                {
                    cityList.Id = reader3.GetInt32(reader3.GetOrdinal("id_city"));
                }
                reader3.Close();
                command.ExecuteNonQuery();
                connection.Close();
            }    
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Insert into [Events_list]([name], [date], [id_city]) values(@1, @2, @3)", connection);
                command.Parameters.Add("@1", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                command.Parameters.Add("@2", SqlDbType.DateTime).Value = DatePicker.Text;
                command.Parameters.Add("@3", SqlDbType.Int).Value = cityList.Id;
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Данные были добавлены");
            NavigationService.GoBack();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Update City_list Set [city_name] = @1 Where id_city = @2 ", connection))
                {
                   
                    command.Parameters.Add("@1",SqlDbType.NVarChar).Value = CityNameTextBox.Text;
                    command.Parameters.Add("@2", SqlDbType.Int).Value = _result.IdCity; 
                    command.ExecuteNonQuery();
                }
                using(SqlCommand command = new SqlCommand("Update Events_list Set [name]=@1 Where id_event =@2", connection))
                {
                    command.Parameters.Add("@1", SqlDbType.NVarChar).Value = EventNameTextBox.Text;
                    command.Parameters.Add("@2", SqlDbType.Int).Value = _result.IdEvent;
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand("Update Events_list Set [date]=@1 Where id_event =@2", connection))
                {
                    command.Parameters.Add("@1", SqlDbType.DateTime).Value =DatePicker.Text;
                    command.Parameters.Add("@2", SqlDbType.Int).Value = _result.IdEvent;
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Изменение было выполнено");
            NavigationService.GoBack();
        }
    }  
}
