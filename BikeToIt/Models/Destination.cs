using System;
namespace BikeToIt.Models
{
    public class Destination
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
        public string Image { get; set; }

        //Creates foreign key in SQL
        public int CategoryId { get; set; }
        public DestinationCategory Category { get; set; }

        //Creates foreign key in SQL
        public int TrailId { get; set; }
        public Trail Trail { get; set; }


        //Picture

        public Destination()
        {
        }

        public Destination(string name, string street, string city, string state, string zipcode, string description, string website,
           bool outdoorSeating, bool bikeRacks, bool restrooms, bool playground, string image)
        {
            Name = name;
            Street = street;
            City = city;
            State = state;
            Zipcode = zipcode;
            Description = description;
            Website = website;
            OutdoorSeating = outdoorSeating;
            BikeRacks = bikeRacks;
            Restrooms = restrooms;
            Playground = playground;
            Image = image;
        }
    }
}
