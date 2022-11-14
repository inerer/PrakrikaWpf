using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Models
{
    public class User
    {
        public PartyMembers? IdMember { get; set; }
        public Role? RoleMember { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
