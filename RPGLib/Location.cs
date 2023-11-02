using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
    public class Location
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Villager> Inhabitants { get; set; }

        public Location(string name, string description)
        {
            Name = name;
            Description = description;
            Inhabitants = new List<Villager>();
        }
    }
}
