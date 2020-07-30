using System;
namespace BikeToIt.Models
{
    public class TrailCity
    {
        public int TrailId { get; set; }
        public Trail Trail { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }


        public TrailCity()
        {
        }
    }
}
