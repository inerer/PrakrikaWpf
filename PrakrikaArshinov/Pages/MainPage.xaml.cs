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
using System.Configuration;
using PrakrikaArshinov.Models;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace PrakrikaArshinov.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        string search;
        PartyMembers partyMembers1;
        string connectionString;
        SqlDataAdapter adapter;
        List<Result> editResult = new List<Result>();
        List<Result> results = new List<Result>();
        
        public MainPage()
        {
            try
            {
                InitializeComponent();
                connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }
        public MainPage(PartyMembers partyMembers)
        {
            try
            {
                InitializeComponent();
                connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
                partyMembers1 = partyMembers;
                this.DataContext = partyMembers1;
                search = "";
                if (partyMembers1.IdRank != 3)
                {
                    RegisterButton.IsEnabled = false;
                    EditButton.IsEnabled = false;
                    AddButton.IsEnabled = false;
                    DeleteButton.IsEnabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }
        public void FullInfo()
        {
            try
            {
                adapter = new SqlDataAdapter();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select Events_list.id_event, City_list.id_city, City_list.city_name, Events_list.name, Events_list.date\r\nFrom  [dbo].[Events_list] \r\njoin City_list on Events_list.id_city = City_list.id_city", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    results.Clear();
                    while (reader.Read())
                    {
                        Result result = new Result();
                        result.CityName = reader.GetString(reader.GetOrdinal("city_name"));
                        result.EventName = reader.GetString(reader.GetOrdinal("name"));
                        result.date = reader.GetDateTime(reader.GetOrdinal("date"));
                        result.IdEvent = reader.GetInt32(reader.GetOrdinal("id_event"));
                        result.IdCity = reader.GetInt32(reader.GetOrdinal("id_city"));

                        results.Add(result);

                    }
                    ResultDataGrid.ItemsSource = null;
                    ResultDataGrid.ItemsSource = results;
                    connection.Close();
                }
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка");
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ResultDataGrid.ItemsSource = null;
            FullInfo();
            List<CityList> cityLists = new List<CityList>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from City_list", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CityList cityList = new CityList();
                    cityList.Name = reader.GetString(reader.GetOrdinal("city_name"));
                    cityLists.Add(cityList);
                }
                connection.Close();
                FilterComboBox.ItemsSource = cityLists;
            }
        }
        private void AgentDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void AgentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrPage());
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = ResultDataGrid.SelectedItem as Result;
                if (selectedItem != null)
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить?", "подтвердите удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        int id = selectedItem.IdCity;
                        int id1 = selectedItem.IdEvent;
                        results.Remove(selectedItem);
                        ResultDataGrid.ItemsSource = null;
                        ResultDataGrid.ItemsSource = results;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command3 = new SqlCommand("Delete From Visit_log Where [id_event]=@id3", connection))
                            {
                                command3.CommandType = CommandType.Text;
                                command3.Parameters.AddWithValue("@id3", id1);
                                command3.ExecuteNonQuery();
                            }
                            using (SqlCommand command2 = new SqlCommand("Delete From Events_list Where[id_city]=@id2", connection))
                            {
                                command2.CommandType = CommandType.Text;
                                command2.Parameters.AddWithValue("@id2", id);
                                command2.ExecuteNonQuery();
                            }
                            using (SqlCommand command = new SqlCommand("Delete From City_list Where [id_city]=@id", connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                            }
                            using (SqlCommand command1 = new SqlCommand("Delete From Events_list Where [id_event]=@id1", connection))
                            {
                                command1.Parameters.AddWithValue("@id1", id1);
                                command1.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Запись удалена");
                    }
                }
                else
                    MessageBox.Show("Пользователь не выбран!");
            }
            catch
            {
                MessageBox.Show("Запись есть в других таблицах!");
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage());
        }
        private void SearchingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search = SearchingTextBox.Text;
            if (search == "")
                FullInfo();
            else
                RenderGrid();
        }
        private void RenderGrid()
        {
            if (search == String.Empty)
                editResult = results;
            else
            editResult = results.Where(c => c.CityName.Contains(search) || c.EventName.Contains(search)).ToList();
            ResultDataGrid.ItemsSource = null;
            ResultDataGrid.ItemsSource = editResult;
        }
        private void SortByDate_Checked(object sender, RoutedEventArgs e)
        {
            ResultDataGrid.ItemsSource = results.OrderBy(c => c.date);
        }
        private void DeleteSortRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            FullInfo();
        }
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityList city = FilterComboBox.SelectedItem as CityList;
            if (city != null) ResultDataGrid.ItemsSource = results.Where(c => c.CityName == city.Name);
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            search = SearchingTextBox.Text;
            if (ResultDataGrid.Items.Count > 0)
            {
                if (search != "")
                {
                    using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "print.rtf"))
                    {
                            foreach (var item in editResult)
                            {
                                string str = $"{item.CityName} {item.EventName} {item.date}   \n";
                                sw.WriteLine(str);
                            }
                            MessageBox.Show("Файл был собран");
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "print.rtf"))
                    {

                        string[] strings = new string[results.Count];
                            foreach (var item in results)
                            {
                            int i = 0;
                            string str = $"{item.CityName} {item.EventName} {item.date}   \n";
                            sw.WriteLine(str);
                            strings[i] = str;
                            i++;
                                
                            }
                            MessageBox.Show("Файл был собран");    
                    }
                }
            }
            else
                MessageBox.Show("Данных нет");
        }
            private void EditButton_Click(object sender, RoutedEventArgs e)
            {
            Result result = new Result();
            result = ResultDataGrid.SelectedItem as Result;
            if (result != null)
            {
                NavigationService.Navigate(new AddPage(result));
            }
            else
                MessageBox.Show("Выберите поле для редактирования");
            }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
