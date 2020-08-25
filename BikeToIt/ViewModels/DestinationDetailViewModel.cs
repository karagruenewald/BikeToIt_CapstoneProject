using System;
using System.Collections.Generic;
using BikeToIt.Models;

namespace BikeToIt.ViewModels
{
    public class DestinationDetailViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public bool OutdoorSeating { get; set; }
        public bool BikeRacks { get; set; }
        public bool Restrooms { get; set; }
        public bool Playground { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TrailId { get; set; }
        public Trail Trail { get; set; }
        public string TrailName { get; set; }
        public string Image { get; set; }

        public DestinationDetailViewModel(Destination theDestination, DestinationCategory category, Trail trail)
        {
            Id = theDestination.Id;
            Name = theDestination.Name;
            Street = theDestination.Street;
            City = theDestination.City;
            State = theDestination.State;
            Zipcode = theDestination.Zipcode;
            Description = theDestination.Description;            
            OutdoorSeating = theDestination.OutdoorSeating;
            BikeRacks = theDestination.BikeRacks;
            Restrooms = theDestination.Restrooms;
            Playground = theDestination.Playground;
            CategoryId = theDestination.CategoryId;
            CategoryName = category.Name;
            TrailId = theDestination.TrailId;
            TrailName = trail.Name;
            Image = theDestination.Image;

            if(theDestination.Website != null && theDestination.Website != "")
            {
                if (!theDestination.Website.StartsWith("http"))
                {
                    Website = "https://" + theDestination.Website;
                }
                else
                {
                    Website = theDestination.Website;
                }
            }
            else
            {
                Website = null;
            }

           
            
        }

        public DestinationDetailViewModel()
        {

        }
    }
}
