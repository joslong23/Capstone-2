using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    public class Program
    {
        public static void Main(string[] args)
        {


            #region call to appsettings.json for configuration
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            #endregion

            string connectionString = configuration.GetConnectionString("Project");

            VenueSqlDAO venueDAO = new VenueSqlDAO(connectionString);
            SpaceSqlDAO spaceDAO = new SpaceSqlDAO(connectionString);
            ReservationSqlDAO reservationDAO = new ReservationSqlDAO(connectionString);

            UserInterface ui = new UserInterface(venueDAO, spaceDAO, reservationDAO);
            ui.Run();

        }
    }
}
