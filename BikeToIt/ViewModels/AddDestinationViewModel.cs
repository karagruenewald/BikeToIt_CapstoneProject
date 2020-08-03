using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BikeToIt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BikeToIt.ViewModels
{
    public class AddDestinationViewModel
    {

        [Required(ErrorMessage="Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required.")]
        public string Zipcode { get; set; }

        public string Description { get; set; }

        
        public string Website { get; set; }

        [Display(Name="Outdoor Seating")]
        public bool OutdoorSeating { get; set; }

        [Display(Name= "Bike Racks")]
        public bool BikeRacks { get; set; }
        public bool Restrooms { get; set; }
        public bool Playground { get; set; }

        [Required(ErrorMessage ="Category is required.")]
        public int CategoryId { get; set; }
        public List<SelectListItem> Category { get; set; }


        [Required(ErrorMessage = "Trail is required.")]
        public int TrailId { get; set; }
        public List<SelectListItem> Trail { get; set; }
        
        public string Image { get; set; }


        public AddDestinationViewModel(List<DestinationCategory> categories, List<Trail> trails)
        {

            Category = new List<SelectListItem>();

            foreach(var c in categories)
            {
                Category.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
            }
            Trail = new List<SelectListItem>();

            foreach (var t in trails)
            {
                Trail.Add(new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });

            }
        }

        public AddDestinationViewModel()
        {

        }
    }
}
