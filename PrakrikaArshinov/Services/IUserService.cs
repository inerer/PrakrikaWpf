using PrakrikaArshinov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Services
{
    public interface IUserService
    {
        public List<User> GetUser();
        public User GetUserByLogin(string login);
        public User GetUserById(int IdMembers);
        public List<Role> GetRoles();
        public Role GetRoleById(int id);
    }
}
