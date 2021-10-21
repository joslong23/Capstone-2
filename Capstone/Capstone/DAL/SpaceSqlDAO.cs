using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SpaceSqlDAO
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

                            if (reader["monthOpen"] != DBNull.Value)
                            {
                                space.SpaceOpenFrom = Convert.ToInt32(reader["monthOpen"]);
                            }
                            if (reader["monthClosed"] != DBNull.Value)
                            {
                                space.SpaceOpenTo = Convert.ToInt32(reader["monthClosed"]);
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

    }
}
