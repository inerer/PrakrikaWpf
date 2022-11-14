using PrakrikaArshinov.Models;
using PrakrikaArshinov.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrakrikaArshinov.U
{
    public class UserRepository : IUserService
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
         public UserRepository (string connectionString)
        {
            _connectionString = connectionString;
            try
            {
                _connection = new SqlConnection(_connectionString);
            }
            catch { MessageBox.Show(""); }
        }

        public Role GetRoleById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Role> GetRoles()
        {
            throw new NotImplementedException();
        }

        public List<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int IdMembers)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
