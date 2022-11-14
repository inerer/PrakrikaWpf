using PrakrikaArshinov.Models;
using PrakrikaArshinov.U;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Services
{
    public class UserService:IUserService
    {
        private readonly string _connectionString;
        private UserRepository _userRepository;

        public UserService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Party"].ConnectionString;
            _userRepository = new UserRepository(_connectionString);
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
