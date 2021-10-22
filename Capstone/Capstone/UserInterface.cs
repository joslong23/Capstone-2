using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        const string Command_ListVenues = "1";
        const string Command_SelectVenues = "2";
        const string Command_ViewSpaces = "1";
        const string Command_SearchForReservation = "2";
        const string Command_ReturnToPreviousScreen = "R";
        const string Command_ReserveSpace = "1";
        const string Command_Quit = "Q";

        private readonly VenueSqlDAO venueDAO;

        private readonly SpaceSqlDAO spaceDAO;

        private readonly ReservationSqlDAO reservationDAO;
        public UserInterface(VenueSqlDAO venueDAO, SpaceSqlDAO spaceDAO, ReservationSqlDAO reservationDAO)
        {
            this.venueDAO = venueDAO;
            this.spaceDAO = spaceDAO;
            this.reservationDAO = reservationDAO;
        }

        public void Run()
        {

            PrintMainMenu();

            while (true)
            {
                string input = Console.ReadLine();

                Console.Clear();

                switch (input.ToUpper())
                {
                    case Command_ListVenues:
                        GetAllVenues();
                        SelectVenue();
                        break;
                  
                    case Command_Quit:
                        Console.WriteLine("Thank you for using Excelsior Venues");
                        return;
                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }

                PrintMainMenu();
            }
        }

        private void PrintMainMenu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit Program");
        }

        private void GetAllVenues()
        {
           List<Venue> venues = venueDAO.GetAllVenues();
             
           if (venues.Count > 0)
           {
                foreach(Venue ven in venues)
                {
                    Console.WriteLine($"{ven.VenueId}) {ven.VenueName} ");
                }
                Console.WriteLine();
           }
            else
            {
                Console.WriteLine("*** No Results ***");
            }
        }

        private void SelectVenue()
        {
            string venueID = CLIHelper.GetString("Which Venue would you like to view? ");
            int intValue;
            List<Venue> venues  = venueDAO.GetAllVenues();

            if (venueID.Contains("r") || venueID.Contains("R")) 
            {
                return;
            }
            else
            {
                intValue = int.Parse(venueID);
                foreach(Venue ven in venues)
                {
                    if(intValue == ven.VenueId)
                    {
                        DisplayVenueDetails(ven);
                    }
                }
            }
        }

        private void DisplayVenueDetails(Venue venue)
        {
            Console.WriteLine($"{venue.VenueName}");
            Console.WriteLine($"Location: {venue.VenueCity}, {venue.VenueState}");
            Console.WriteLine($"Categories: {venue.VenueCategory}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Description: ");
            Console.WriteLine($"{venue.VenueDescription}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("R) Return to Previous Screen");
            Console.WriteLine();

            string input = CLIHelper.GetString("");

            int intValue;

            if (input.Contains("r") || input.Contains("R"))
            {
                return;
            }
            else if (input == "1")
            {
                ViewSpaces(venue.VenueId);
            }

        }

        private void ViewSpaces(int venueID)
        {
            List<Spaces> spaces = spaceDAO.GetVenueSpaces(venueID);

            Console.WriteLine("Name---- ---- ---- Open ---- Close ---- Daily Rate ---- Max. Occupancy");

            foreach (Spaces space in spaces)
            {
                // Yup. I added a static method. Sue me. This pulls the abbreviated month name from an object property for display
                string openMonth = CLIHelper.GetAbbreviatedMonthName(space.SpaceOpenFrom);
                string closedMonth = CLIHelper.GetAbbreviatedMonthName(space.SpaceOpenTo);

                Console.WriteLine($"#{space.SpaceId} ---- {space.SpaceName} ---- ---- {openMonth} ---- {closedMonth} ---- {space.SpaceDailyRate} ---- {space.SpaceMaxOccupancy}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("    1) Reserve a Space \n    R) Return to Previous Screen ");

            string input = CLIHelper.GetString("");


            if (input.Contains("r") || input.Contains("R"))
            {
                return;
            }
            else if (input == "1")
            {
                int space = spaces.First().SpaceId;
                ReserveSpace(space);
            }
        }

        private void ReserveSpace(int spaceID)
        {

            DateTime reservedDate = CLIHelper.GetDateTime("Enter the date you would like to Reserve: ");

            int daysNeeded = CLIHelper.GetInteger("How many days will you be reserving?: ");

            DateTime reservationEndDate = reservedDate.AddDays(daysNeeded);
            
            int attendanceCount = CLIHelper.GetInteger("How many guests will be attending?: ");

            string reservingParty = CLIHelper.GetString("Who is the reserving Person or Party?: ");
            Console.WriteLine("The following spaces are available based on your needs: ");





            reservationDAO.ReserveSpace(reservedDate, reservationEndDate, attendanceCount, reservingParty, spaceID);

            

        }

    }
}
