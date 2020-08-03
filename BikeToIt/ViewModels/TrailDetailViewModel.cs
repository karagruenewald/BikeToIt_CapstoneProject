using System;
using System.Collections.Generic;
using BikeToIt.Models;

namespace BikeToIt.ViewModels
{
    public class TrailDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public double Distance { get; set; }
        public string SurfaceType { get; set; }
        public string Description { get; set; }
        public List<TrailCity> Cities { get; set; }
        public List<Destination> Destinations { get; set; }
        public string Image { get; set; }



        public TrailDetailViewModel(Trail theTrail, List<Destination> destinations, List<TrailCity> cities)
        {
            Id = theTrail.Id;
            Name = theTrail.Name;
            State = theTrail.State;
            Distance = theTrail.Distance;
            SurfaceType = theTrail.SurfaceType;
            Description = theTrail.Description;
            Destinations = destinations;
            Cities = cities;
            Image = theTrail.Image;
            

        }

        public TrailDetailViewModel()
        {

        }
    }
}
