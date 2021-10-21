using Capstone.DAL;
using System;
using System.Collections.Generic;
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
        const string Command_ViewSpaces = "1";
        const string Command_SearchForReservation = "2";
        const string Command_ReturnToPreviousScreen = "R";
        const string Command_ReserveSpace = "1";
        const string Command_Quit = "Q";



        private readonly string connectionString;

        private readonly VenueDAO venueDAO;


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
        }

        public void Run()
        {
            PrintMainMenu();

            string input = Console.ReadLine();

            while (true)
            {

                switch (input.ToUpper())
                {
                    case Command_ListVenues:
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
            /* List<VenueDAO> venues = VenueSqlDAO.GetAllVenues();
             
            if (venues.Count > 0)
            {
                foreach (Venue ven in venues)
                {
                    Console.WriteLine()


            */
        }

    }
}
