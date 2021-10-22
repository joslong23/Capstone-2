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
            "(space_id, start_date, end_date, number_of_attendees, reserved_for) " +
            "VALUES (@space_id, @start_date , @end_date, @number_of_attendees, @reserved_for); " +
            "SELECT @@IDENTITY;";

        private readonly string SqlDisplayNewReservation =
            "SELECT v.name AS venueName, s.name AS spaceName, s.daily_rate AS dailyRate " +
            "FROM reservation r " +
            "INNER JOIN space s ON s.id = r.space_id " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE r.reservation_id = @reservation_id";

        private readonly string SqlMaxOccupancyCheck =
            "SELECT s.max_occupancy " +
            "FROM space s " +
            "INNER JOIN reservation r ON r.space_id = s.id  " +
            "WHERE s.id = @space_id; ";
        private readonly string SqlDateCheck =
            "SELECT start_date , end_date FROM reservation WHERE space_id = @space_id AND @start_date AND NOT BETWEEN start_date AND end_date; ";

        private readonly string SqlAvailableSpaceList =
           "SELECT DISTINCT " +
            "s.id, " +
            "s.name, " +
            "s.daily_rate, " +
            "s.max_occupancy, " +
            "s.is_accessible " +
            "FROM space s " +
            "LEFT OUTER JOIN reservation r ON s.id = r.space_id " +
            "LEFT OUTER JOIN venue v ON v.id = s.venue_id " +
            "WHERE v.id = @venue_id AND r.start_date NOT BETWEEN @start_date AND @end_date " +
            "AND r.end_date NOT BETWEEN @start_date AND @end_date " +
            "OR r.end_date IS NULL AND v.id = @venue_id AND s.max_occupancy >= @number_of_attendees;";
        private readonly string SqlDebug = "SELECT DISTINCT s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible FROM space s LEFT OUTER JOIN reservation r ON s.id = r.space_id LEFT OUTER JOIN venue v ON v.id = s.venue_id WHERE v.id = 1 AND r.start_date NOT BETWEEN '2021-10-15' AND '2021-10-20' AND r.end_date NOT BETWEEN '2021-10-15' AND '2021-10-20' OR r.end_date IS NULL AND v.id = 1";


        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }



        public List<Spaces> GetAvailableReservations(DateTime startDate, DateTime endDate, int daysNeeded, int guestCount, int venueID)
        {
            List<Spaces> result = new List<Spaces>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlDebug, conn);
                    command.Parameters.AddWithValue("@venue_id", venueID);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
                    command.Parameters.AddWithValue("@number_of_attendees", guestCount);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Spaces space = new Spaces();
                        {
                            space.SpaceId = Convert.ToInt32(reader["id"]);
                            space.SpaceName = Convert.ToString(reader["name"]);
                            space.SpaceIsAccessible = Convert.ToBoolean(reader["is_accessible"]);
                            space.SpaceDailyRate = Convert.ToDecimal(reader["daily_rate"]);
                            space.SpaceMaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                            space.TotalCost *= daysNeeded;
                        }
                        result.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not obtain data: " + ex.Message);
            }

            return result;
        }

        public Reservation MakeReservation(DateTime startDate, DateTime endDate, int guestCount, int daysNeeded, int spaceID, string reservationName)
        {
            Reservation reservation = new Reservation();
            try
            {
                int reservationID;
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlReservation, conn);
                    command.Parameters.AddWithValue("@space_id", spaceID);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
                    command.Parameters.AddWithValue("@number_of_attendees", guestCount);
                    command.Parameters.AddWithValue("@reserved_for", reservationName);

                    reservationID = Convert.ToInt32(command.ExecuteScalar());

                }

                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlDisplayNewReservation, conn);
                    command.Parameters.AddWithValue("@reservation_id", reservationID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        reservation.ReservationSpaceName = Convert.ToString(reader["spaceName"]);
                        reservation.ReservationVenueName = Convert.ToString(reader["venueName"]);
                        reservation.TotalCost = Convert.ToDecimal(reader["dailyRate"]);
                    }

                    reservation.ReservationStartDate = startDate;
                    reservation.ReservationEndDate = endDate;
                    reservation.ReservationAttendees = guestCount;
                    reservation.ReservationId = reservationID;
                    reservation.ReservationReservedFor = reservationName;
                    reservation.TotalCost *= daysNeeded;
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not add data to database: " + ex.Message);
            }
            return reservation;
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

                    SqlDataReader reader = occupancyCheck.ExecuteReader();

                    int maxOccupancyChecker = 0;

                    while (reader.Read())
                    {
                        int maxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

                        maxOccupancyChecker += maxOccupancy;
                        break;
                    }

                    if (maxOccupancyChecker < numberOfGuests)
                    {
                        //reservation.IsValidGuestCount = false;
                        return reservation;
                    }

                    SqlCommand dateCheck = new SqlCommand(SqlDateCheck, conn);
                    dateCheck.Parameters.AddWithValue("@space_id", spaceId);

                    DateTime reservedDate = Convert.ToDateTime(dateCheck.ExecuteScalar());
                    if (reservedDate == DateTime.MinValue)
                    {
                        // reservation.IsValidDate = false;
                        return reservation;
                    }

                    SqlCommand command = new SqlCommand(SqlReservation, conn);

                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);
                    command.Parameters.AddWithValue("@number_of_attendees", numberOfGuests);
                    command.Parameters.AddWithValue("@reserved_for", reservationName);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    reservation.ReservationId = id;
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not access data form the database: " + ex.Message);
            }

            return reservation;
        }
    }
}
