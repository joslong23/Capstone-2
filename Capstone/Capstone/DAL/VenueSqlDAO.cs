using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueSqlDAO
    {
        private readonly string connectionString;

        private readonly string SqlVenueList =
            "SELECT id,  name FROM venue ";

        public VenueSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlVenueList, conn);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Venue venue = new Venue
                        {
                            VenueId = Convert.ToInt32(reader["id"]),
                            VenueName = Convert.ToString(reader["name"]),
                            VenueCityId = Convert.ToInt32(reader["city_id"]),
                            VenueDescription = Convert.ToString(reader["description"])
                        };
                        venues.Add(venue);
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Could not obtain data from database: " + ex.Message);
            }
            return venues;
        }





    }
}
