using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class Publisher
    {
        public string Name { get; set; }
        public string Country { get; set; }

        public Publisher(string name, string country)
        {
            Name = name;
            Country = country;
        }
    }
}
