using System;
using System.Collections.Generic;
using BikeToIt.Models;

namespace BikeToIt.ViewModels
{
    public class AllUserDestinations
    {

        public List<UserDestination> NotYetApprovedDestinations { get; set; }
        public List<Destination> ApprovedDestinations { get; set; }



        public AllUserDestinations(List<UserDestination> notYetApprovedDestinations, List<Destination> approvedDestinations)
        {

            NotYetApprovedDestinations = notYetApprovedDestinations;
            ApprovedDestinations = approvedDestinations;
        }


        public AllUserDestinations()
        {
        }
    }
}
