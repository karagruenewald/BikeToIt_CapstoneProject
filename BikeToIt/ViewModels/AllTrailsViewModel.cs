using System;
using System.Collections.Generic;
using BikeToIt.Models;

namespace BikeToIt.ViewModels
{
    public class AllTrailsViewModel
    {

        public List<Trail> Trails { get; set; }
        public List<TrailCity> TrailCity { get; set; }

        public AllTrailsViewModel(List<Trail> trails, List<TrailCity> cities)
        {

            Trails = trails;
            TrailCity = cities;


        }
        public AllTrailsViewModel()
        {

        }
    }
}
