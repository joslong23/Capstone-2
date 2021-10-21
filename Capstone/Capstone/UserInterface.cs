using Capstone.DAL;
using Capstone.Models;
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
        const string Command_SelectVenues = "2";
        const string Command_ViewSpaces = "1";
        const string Command_SearchForReservation = "2";
        const string Command_ReturnToPreviousScreen = "R";
        const string Command_ReserveSpace = "1";
        const string Command_Quit = "Q";

        private readonly VenueSqlDAO venueDAO;


        public UserInterface(VenueSqlDAO venueDAO)
        {
            this.venueDAO = venueDAO;
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
                    case Command_SelectVenues:
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
            Console.WriteLine("2) Select Venue");
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
            int venueID = CLIHelper.GetInteger("Which Venue would you like to view? ");
            

        }
    }
}
