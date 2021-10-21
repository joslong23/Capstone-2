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
            "SELECT " +
            "v.id, " +
            "v.name, " +
            "STRING_AGG( ISNULL(c.name, ' '), ', ') AS categoryName, " +
            "v.description " +
            "FROM venue v " +
            "INNER JOIN category_venue cv ON v.id=cv.venue_id " +
            "INNER JOIN category c ON c.id = cv.category_id " +
            "GROUP BY v.id, v.name, v.description";

        public VenueSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();

            try
            {
                CreateVenueList(venues);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not obtain data from database: " + ex.Message);
            }
            return venues;
        }
        public List<Venue> GetVenueDetails()
        {
            List<Venue> venues = new List<Venue>();

            try
            {
                CreateVenueList(venues);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not obtain data from database: " + ex.Message);
            }
            return venues;
        }
        private void CreateVenueList(List<Venue> venues)
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
                        VenueCategory = Convert.ToString(reader["categoryName"]),
                        VenueDescription = Convert.ToString(reader["description"])
                    };
                    venues.Add(venue);
                }
            }
        }




    }
}
