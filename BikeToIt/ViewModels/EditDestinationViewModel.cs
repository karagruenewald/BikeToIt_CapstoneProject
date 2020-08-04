using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BikeToIt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BikeToIt.ViewModels
{
    public class EditDestinationViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
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

        [Display(Name = "Outdoor Seating")]
        public bool OutdoorSeating { get; set; }

        [Display(Name = "Bike Racks")]
        public bool BikeRacks { get; set; }
        public bool Restrooms { get; set; }
        public bool Playground { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        public List<SelectListItem> Category { get; set; }


        [Required(ErrorMessage = "Trail is required.")]
        public int TrailId { get; set; }
        public List<SelectListItem> Trail { get; set; }

        public IFormFile Image { get; set; }

        public string ExistingImage { get; set; }

        public EditDestinationViewModel()
        {

        }

        public EditDestinationViewModel(Destination destination, List<DestinationCategory> categories, List<Trail> trails)
        {
            Id = destination.Id;
            Name = destination.Name;
            Street = destination.Street;
            City = destination.City;
            State = destination.State;
            Zipcode = destination.Zipcode;
            Description = destination.Description;
            Website = destination.Website;
            OutdoorSeating = destination.OutdoorSeating;
            BikeRacks = destination.BikeRacks;
            Restrooms = destination.Restrooms;
            Playground = destination.Playground;
            ExistingImage = destination.Image;
            CategoryId = destination.CategoryId;
            TrailId = destination.TrailId;
            

            Category = new List<SelectListItem>();

            foreach (var c in categories)
            {
                Category.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
            
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
    }
}
