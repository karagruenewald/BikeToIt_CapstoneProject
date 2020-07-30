using System;
using System.Collections.Generic;

namespace BikeToIt.Models
{
    public class DestinationCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Needs a reference to Destinations that have categories
        public List<Destination> Destinations { get; set; }


        public DestinationCategory()
        {
        }

        public DestinationCategory(string name)
        {
            Name = name;
        }
    }
}
