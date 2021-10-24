using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public int VenueCityId { get; set; }
        
        public string VenueDescription { get; set; }
       
        public string VenueCategory { get; set; }

        public string VenueCity { get; set; }

        public string VenueState { get; set; }
    }
}
