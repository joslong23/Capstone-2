using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {

        public int ReservationId { get; set; }

        public string ReservationVenueName { get; set; }

        public string ReservationSpaceName { get; set; }

        public int ReservationAttendees { get; set; }

        public DateTime ReservationStartDate { get; set; }

        public DateTime ReservationEndDate { get; set; }

        public string ReservationReservedFor { get; set; }

        public decimal TotalCost { get; set; } // This is not held in the data base but needs to be displayed to the user


    }
}
