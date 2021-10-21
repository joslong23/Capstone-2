using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {
        // Venue Id number 1-15
        public int VenueId { get; set; }

        //Venue name
        public string VenueName { get; set; }

        //Venue city Id 1-4
        public int VenueCityId { get; set; }
        //Venue description
        public string VenueDescription { get; set; }
        /// <summary>
        /// Venue Category
        /// </summary>
        public string VenueCategory { get; set; }

    }
}
