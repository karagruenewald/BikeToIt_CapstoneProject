using System;
using System.Collections.Generic;
using BikeToIt.Models;

namespace BikeToIt.ViewModels
{
    public class SearchResultsViewModel
    {

        public List<Trail> Trails { get; set; }
        public List<TrailCity> TrailCity { get; set; }
        


        public SearchResultsViewModel(List<Trail> trails, List<TrailCity> trailCities) 
        {

            Trails = trails;
            TrailCity = trailCities;
            
        }

        public SearchResultsViewModel()
        {

        }
    }
}
