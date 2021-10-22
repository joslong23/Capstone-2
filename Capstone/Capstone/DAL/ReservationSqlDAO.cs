using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO
    {
        private readonly string connectionString;

        private readonly string SqlReservation = "INSERT INTO reservation " + 
            "(start_date , end_date , number_of_attendees, reserved_for) " + 
            "VALUES (@start_date , @end_date, @number_of_attendees, @reserved_for); "+
            "SELECT @@IDENTITY;";
        private readonly string SqlMaxOccupancyCheck =
            "SELECT s.max_occupancy " +
            "FROM space s " +
            "INNER JOIN reservation r ON r.space_id = s.id  " +
            "WHERE s.id = @space_id;";
        private readonly string SqlDateCheck =
            "SELECT start_date , end_date FROM reservation WHERE space_id = @space_id AND @start_date NOT BETWEEN start_date AND end_date; ";
       
        public ReservationSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Reservation ReserveSpace(DateTime startDate, DateTime endDate, int numberOfGuests, string reservationName, int spaceId)
        {
            Reservation reservation = new Reservation();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();


                    SqlCommand occupancyCheck = new SqlCommand(SqlMaxOccupancyCheck, conn);
                    // checking number of guests against max occupancy, if max occupancy is exceeded it will return false and tell the user, otherwise it will allow the user to continue the reservation.
                    occupancyCheck.Parameters.AddWithValue("@space_id", spaceId);
                    int maxOccupancy = Convert.ToInt32("@number_of_attendees", numberOfGuests);
                    occupancyCheck.ExecuteReader();
                    if(maxOccupancy < numberOfGuests)
                    {
                        reservation.IsValidGuestCount = false;
                        return reservation;
                    }

                    SqlCommand dateCheck = new SqlCommand(SqlDateCheck, conn);
                    dateCheck.Parameters.AddWithValue("@space_id", spaceId);

                    DateTime reservedDate = Convert.ToDateTime(dateCheck.ExecuteScalar());
                    if(reservedDate == DateTime.MinValue)
                    {
                        reservation.IsValidDate = false;
                        return reservation ;
                    }

                    SqlCommand command = new SqlCommand(SqlReservation, conn);

                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfGuests);
                    command.Parameters.AddWithValue("@reserved_for", reservationName);

                    int id =Convert.ToInt32(command.ExecuteScalar());

                    reservation.ReservationId = id;
                }

            }
            catch(SqlException ex)
            {
                Console.WriteLine("Could not access data form the database: " + ex.Message);
            }
           
            return reservation;
        }

    }
}
