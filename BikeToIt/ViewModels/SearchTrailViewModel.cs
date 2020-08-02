using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BikeToIt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BikeToIt.ViewModels
{
    public class SearchTrailViewModel
    {
        
        public string Name { get; set; }
        public List<SelectListItem> State { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> City { get; set; }

        
        public double Distance { get; set; }

        [Display(Name="Surface Type")]
        public string SurfaceType { get; set; }
        public int DestinationCategoryId { get; set; }
        public  List<DestinationCategory> DestinationCategory { get; set; }


        public bool DestinationCategoryCheckbox { get; set; }
        



        public SearchTrailViewModel(List<string> states, List<City> cities, List<DestinationCategory> categories)
        {
            State = new List<SelectListItem>();

            foreach(var state in states)
            {
                
                    State.Add(new SelectListItem
                    {
                        Value = state,
                        Text = state
                    });

               
                
            }
            State.Insert(0, new SelectListItem { Value = null, Text = "" });

            City = new List<SelectListItem>();

            foreach (var city in cities)
            {
                City.Add(new SelectListItem
                {
                    Value = city.Id.ToString(),
                    Text = city.Name
                }) ;
            }

            City.Insert(0, new SelectListItem { Value = null, Text = "" });


            DestinationCategory = categories;

           


        }


        public SearchTrailViewModel()
        {
        }
    }
}
