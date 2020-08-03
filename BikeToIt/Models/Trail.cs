using System;
using System.Collections.Generic;

namespace BikeToIt.Models
{
    public class Trail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        
        public double Distance { get; set; }
        public string SurfaceType { get; set; }
        public string Description { get; set; }
        public List<Destination> Destinations { get; set; }
        public string Image { get; set; }


        public Trail()
        {
        }

    }
}
