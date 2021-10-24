using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SpaceSqlDAO : Spaces
    {
        private readonly string connectionString;

        private readonly string SqlSpaceList =
            "SELECT  " +
            "sp.id,  " +
            "sp.name, " +
            "sp.is_accessible, " +
            "sp.open_from AS 'monthOpen', " +
            "sp.open_to AS 'monthClosed', " +
            "sp.daily_rate,  " +
            "sp.max_occupancy " +
            "FROM " +
            "venue v " +
            "INNER JOIN space sp ON sp.venue_id = v.id " +
            "WHERE sp.venue_id = @venue_id";
        private readonly string SqlAvailableSpaceList =
                 "SELECT TOP 5 " +
                   "s.id , s.name, s.daily_rate, s.max_occupancy, s.is_accessible " +
                   "FROM space s " +
                   "WHERE s.venue_id = @venue_id AND s.id NOT IN " +
                   "(SELECT s.id FROM reservation r " +
                   "JOIN space s on s.id = r.space_id " +
                   "WHERE s.venue_id = @venue_id AND r.end_date >= @start_date AND r.start_date <= @end_date " +
                   "AND s.venue_id= @venue_id) AND((MONTH(@start_date)> s.open_from AND MONTH(@start_date) <s.open_to) OR s.open_from IS NULL) " +
                   "AND s.max_occupancy >= @number_of_attendees;";

        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Spaces> GetVenueSpaces(int venueID)
        {
            List<Spaces> result = new List<Spaces>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSpaceList, conn);
                    command.Parameters.AddWithValue("@venue_id", venueID);

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

                            if (reader["monthOpen"] != DBNull.Value) // Sets the date if the row in the data base has one
                            {
                                space.SpaceOpenFrom = Convert.ToInt32(reader["monthOpen"]);
                            }
                            else
                            {
                                space.SpaceOpenFrom = 13; // 13 is the value for a null month. Or it isn't null? Unclear. Either way, it displays nothing when called
                            }
                            if (reader["monthClosed"] != DBNull.Value)
                            {
                                space.SpaceOpenTo = Convert.ToInt32(reader["monthClosed"]);
                            }
                            else
                            {
                                space.SpaceOpenTo = 13;
                            }

                            result.Add(space);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not obtain data: " + ex.Message);
            }
            return result;
        }
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
    }
}
