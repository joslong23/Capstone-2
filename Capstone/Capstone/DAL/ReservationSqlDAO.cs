﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO
    {
        private readonly string connectionString;

        /// <summary>
        /// This string is used to add a reservation to the database
        /// </summary>
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
        /// <summary>
        /// This sql displays available spaces based upon user input. Examines dates and max occupancy
        /// </summary>
        private readonly string SqlAvailableSpaceList =
           "SELECT DISTINCT " +
            "s.id , s.name, s.daily_rate, s.max_occupancy, s.is_accessible " +
            "FROM space s " +
            "LEFT OUTER JOIN reservation r ON r.space_id = s.id " +
            "INNER JOIN venue v ON v.id = s.venue_id " +
            "WHERE v.id = @venue_id AND (r.start_date != @start_date AND r.start_date != @end_date AND r.end_date != @end_date) " +
            "AND r.end_date != @end_date " +
            "AND r.start_date NOT BETWEEN @start_date AND @end_date OR r.start_date IS NULL AND v.id = @venue_id";
        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }


        /// <summary>
        /// Create a list of spaces where a user can place reservation based upon the given info
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="daysNeeded"></param>
        /// <param name="guestCount"></param>
        /// <param name="venueID"></param>
        /// <returns></returns>
        public List<Spaces> GetAvailableReservations(DateTime startDate, DateTime endDate, int daysNeeded, int guestCount, int venueID)
        {
            List<Spaces> result = new List<Spaces>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlAvailableSpaceList, conn);
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
                            space.TotalCost = space.SpaceDailyRate * daysNeeded;
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
        /// <summary>
        /// Inserts a new reservation into the database and returns an instance of a reservation to be used to display to the user
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="guestCount"></param>
        /// <param name="daysNeeded"></param>
        /// <param name="spaceID"></param>
        /// <param name="reservationName"></param>
        /// <returns></returns>
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
                    command.Parameters.AddWithValue("@start_date", startDate.ToShortDateString());
                    command.Parameters.AddWithValue("@end_date", endDate.ToShortDateString());
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
    }
}
