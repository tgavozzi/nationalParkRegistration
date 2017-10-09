using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.CLI;
using Capstone.Models;
using Capstone.DAL;
using System.Globalization;

namespace Capstone
{
    public class NationalParkCLI
    {
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Campground;User ID=te_student;Password=sqlserver1";
        private decimal campgroundPrice;

        public void Run()
        {
            Header();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("WELCOME TO THE NATIONAL PARK REGISTRATION");
                Console.WriteLine();
                Console.WriteLine("1 -- Show me all National Parks with their campgrounds.");
                Console.WriteLine("2 -- I don't need to see the Parks, I already know where I want to stay. Make Reservation!");

                string input = CLIHelper.GetString("Please make a selection: ");

                switch (input.ToLower())
                {
                    case "1":
                        ShowAllNationalParks();
                        break;
                    case "2":
                        int reservationInput = CLIHelper.GetInteger("What Park ID would you like to book for? ");
                        ShowAllCampgroundInAPark(reservationInput);
                        break;
                }
            }
        }

        private void ShowAllNationalParks()
        {
            Console.WriteLine("** Showing Parks **");
            Console.WriteLine();

            ParkSqlDAL dal = new ParkSqlDAL(connectionString);

            List<Park> allParks = dal.GetAllParks();

            foreach (Park p in allParks)
            {
                Console.WriteLine();
                Console.WriteLine("Park ID: " + p.Park_id);
                Console.WriteLine("Park Name: " + p.Name);
                Console.WriteLine("Park Location: " + p.Location);
                Console.WriteLine("Date of Park Establishment: " + p.Establish_date);
                Console.WriteLine("Park Size: " + p.Area + " sq ft");
                Console.WriteLine("Number of vistors in Park: " + p.Vistitors);
                Console.WriteLine("Description of Park: " + p.Description);
            }

            Console.WriteLine();
            Console.WriteLine("Would you Like to see a Parks campgrounds(Y/N)?");
            string input = Console.ReadLine().ToUpper();
            if (input == "Y")
            {
                Console.WriteLine();
                int userInputParkId = CLIHelper.GetInteger("Please enter a Park Id: ");

                ShowAllCampgroundInAPark(userInputParkId);
            }
            return;
        }

        private void ShowAllCampgroundInAPark(int userInputParkId)
        {
            Console.WriteLine("** Showing Campgrounds **");

            CampgroundSqlDAL cal = new CampgroundSqlDAL(connectionString);

            List<Campground> allCampgrounds = cal.GetAllCampgrounds(userInputParkId);

            foreach (Campground c in allCampgrounds)
            {
                Console.WriteLine();
                Console.WriteLine("Campground Id: " + c.Campground_id);
                Console.WriteLine("Park ID: " + c.Park_id);
                Console.WriteLine("Campground Name: " + c.Name);
                Console.WriteLine("Campground is open from: " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Open_from_mm));
                Console.WriteLine("Campground closes in: " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Open_to_mm));
                Console.WriteLine("Campground daily cost: " + c.Daily_fee.ToString("C"));
            }
            Console.WriteLine();
            Console.WriteLine("Would you like to book one of these sites? (Y/N)");
            string input = Console.ReadLine().ToUpper();
            if (input == "Y")
            {
                int campgroundId = CLIHelper.GetInteger("Please select a Campground ID");

                Reservation(campgroundId);
            }
            return;
        }

        private void Reservation(int campgroundId)
        {
            DateTime arrivalInput = CLIHelper.GetDateTime("What is your desired arrival date?(MM/DD/YYYY)");
            DateTime fromDate = Convert.ToDateTime(arrivalInput);

            DateTime departureInput = CLIHelper.GetDateTime("When would you like to leave?(MM/DD/YYYY)");
            DateTime toDate = Convert.ToDateTime(departureInput);

            if (toDate <= fromDate)
            {
                Console.WriteLine();
                Console.WriteLine("Leaving date is prior to arrival date.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();

            Console.WriteLine("** Showing available sites **");

            SiteSqlDAL sal = new SiteSqlDAL(connectionString);
            List<Site> allCampSites = sal.CampsiteAvailability(campgroundId, fromDate, toDate);

            if (allCampSites.Count() <= 0)
            {
                Console.WriteLine("No available campsites! Please make a new selection.");
                return;
            }
            else
            {
                foreach (Site s in allCampSites)
                {
                    Console.WriteLine();
                    Console.WriteLine("SiteId: " + s.SiteID);
                    Console.WriteLine("Campground Id: " + s.CampgroundID);
                    Console.WriteLine("SiteNumber: " + s.SiteNumber);
                    Console.WriteLine("MaxOccupancy: " + s.MaxOccupancy);
                    Console.WriteLine("Handicap Accessible: " + s.Accessible);
                    Console.WriteLine("MaxRvLength: " + s.MaxRvLength);
                    Console.WriteLine("Utilities are available: " + s.Utilities);
                }

                ReservationSqlDAL ral = new ReservationSqlDAL(connectionString);

                campgroundPrice = (ral.CostOfCampground(campgroundId) * (toDate.DayOfYear - fromDate.DayOfYear));

                Console.WriteLine();
                Console.WriteLine("The total fee for these sites are: " + campgroundPrice.ToString("C"));
                Console.WriteLine();

                int campSiteInput = CLIHelper.GetInteger("What Camp site are you booking for?");
                Console.WriteLine();

                string nameInput = CLIHelper.GetString("What name should the reservation be under?");
                Console.WriteLine();

                DateTime createDate = DateTime.Now;
                int wasReservationSuccessful = ral.MakeReservations(campSiteInput, nameInput, fromDate, toDate, createDate);

                if (wasReservationSuccessful > 0)
                {
                    Console.WriteLine("Success!");

                    Console.WriteLine("Here is your conformation ID: " + wasReservationSuccessful);
                }
                else
                {
                    Console.WriteLine("Sorry but that site is alreaday booked. Please try again.");
                }
            }
        }

        public void Header()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"
________   ________  _________  ___  ________  ________   ________  ___               ________  ________  ________  ___  __                 
|\   ___  \|\   __  \|\___   ___\\  \|\   __  \|\   ___  \|\   __  \|\  \             |\   __  \|\   __  \|\   __  \|\  \|\  \            
\ \  \\ \  \ \  \|\  \|___ \  \_\ \  \ \  \|\  \ \  \\ \  \ \  \|\  \ \  \            \ \  \|\  \ \  \|\  \ \  \|\  \ \  \/  /|_          
 \ \  \\ \  \ \   __  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ \   __  \ \  \            \ \   ____\ \   __  \ \   _  _\ \   ___  \         
  \ \  \\ \  \ \  \ \  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ \  \ \  \ \  \____        \ \  \___|\ \  \ \  \ \  \\  \\ \  \\ \  \      
   \ \__\\ \__\ \__\ \__\   \ \__\ \ \__\ \_______\ \__\\ \__\ \__\ \__\ \_______\       \ \__\    \ \__\ \__\ \__\\ _\\ \__\\ \__\");
        }
    }
}
