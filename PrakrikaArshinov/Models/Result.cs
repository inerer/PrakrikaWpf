using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Models
{
    public class Result
    {
        public int IdMembers { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string RankParty { get; set; }
        public string CityName { get; set; }
        public DateTime date { get; set; } 
        public string EventName {get; set; }
        public int IdCity { get; set; }
        public int IdEvent { get; set; }
    }
}
