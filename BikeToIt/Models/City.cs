using System;
using System.Collections.Generic;

namespace BikeToIt.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Trail> Trails { get; set; }

        public City()
        {
        }

        public  City (string name)
        {
            Name = name;
        }
    }
}
