using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Models
{
    public class PartyMembers
    {
        public int IdMembers { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int IdRank { get; set; }
        public string password { get; set; }
        public string login { get; set; }
        public string Photo {get; set; }
    }
}
