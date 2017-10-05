using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.CLI;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone
{
    public class NationalParkCLI
    {
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Campground;User ID=te_student;Password=sqlserver1";

        public void Run()
        {
            while(true)
            {

                Console.WriteLine(@"
________   ________  _________  ___  ________  ________   ________  ___               ________  ________  ________  ___  __                 
|\   ___  \|\   __  \|\___   ___\\  \|\   __  \|\   ___  \|\   __  \|\  \             |\   __  \|\   __  \|\   __  \|\  \|\  \            
\ \  \\ \  \ \  \|\  \|___ \  \_\ \  \ \  \|\  \ \  \\ \  \ \  \|\  \ \  \            \ \  \|\  \ \  \|\  \ \  \|\  \ \  \/  /|_          
 \ \  \\ \  \ \   __  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ \   __  \ \  \            \ \   ____\ \   __  \ \   _  _\ \   ___  \         
  \ \  \\ \  \ \  \ \  \   \ \  \ \ \  \ \  \\\  \ \  \\ \  \ \  \ \  \ \  \____        \ \  \___|\ \  \ \  \ \  \\  \\ \  \\ \  \      
   \ \__\\ \__\ \__\ \__\   \ \__\ \ \__\ \_______\ \__\\ \__\ \__\ \__\ \_______\       \ \__\    \ \__\ \__\ \__\\ _\\ \__\\ \__\");


                Console.WriteLine("Welcome (I know our website once sucked...but now it's awesome!!)");
                Console.WriteLine();
                Console.WriteLine("1 - Show all National Parks with their campgrounds");

                string input = CLIHelper.GetString("Please make your selection");

                switch(input.ToLower())
                {
                    case "1":
                        ShowAllNationalParks();
                        break;
                }
            }
        }

        private void ShowAllNationalParks()
        {
            Console.WriteLine("Showing Parks");

            ParkSqlDAL dal = new ParkSqlDAL(connectionString);

            List<Park> allParks = dal.GetAllParks();

            foreach(Park p in allParks)
            {
                Console.WriteLine();
                Console.WriteLine("Park ID: " + p.Park_id);
                Console.WriteLine("Park Name: " + p.Name);
                Console.WriteLine("Park Location:" + p.Location);
                Console.WriteLine("Date of Park Establishment: " + p.Establish_date);
                Console.WriteLine("Park Size: " + p.Area);
                Console.WriteLine("Number of vistors in Park: " + p.Vistitors);
                Console.WriteLine("Description of Park: " + p.Description);
            }

            Console.WriteLine();
            Console.WriteLine("Would you Like to see a Parks campgrounds(Y/N)?");
            string input = Console.ReadLine().ToUpper();
            if(input == "Y")
            {
                Console.WriteLine("Please select a Park ID");
                string userInput = Console.ReadLine();
                int userInputParkId = Convert.ToInt32(userInput);
                ShowAllCampgroundInAPark(userInputParkId);
            }
            return;
        }


        private void ShowAllCampgroundInAPark(int userInputParkId)
        {
            Console.WriteLine("Showing Campgrounds");

            CampgroundSqlDAL cal = new CampgroundSqlDAL(connectionString);

            List<Campground> allCampgrounds = cal.GetAllCampgrounds(userInputParkId);
    
            foreach (Campground c in allCampgrounds)
            {
                Console.WriteLine();
                Console.WriteLine("Campground_Id " + c.Campground_id);
                Console.WriteLine("Park ID: " + c.Park_id);
                Console.WriteLine("Campground Name: " + c.Name);
                Console.WriteLine("Campground is open from " + c.Open_from_mm);
                Console.WriteLine("Campground closes " + c.Open_to_mm);
                Console.WriteLine("The daily fee for this site is " + c.Daily_fee);
            }

            Console.ReadLine();
        }
    }
}
