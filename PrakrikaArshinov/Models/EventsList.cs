using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrakrikaArshinov.Models
{
    public class EventsList
    {
        public int IdEvent { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public CityList? IdCity { get; set; }
    }
}
