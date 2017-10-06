using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;
using Capstone.DAL;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {

        private string connectionString;
        private List<Campground> output = new List<Campground>();

        public CampgroundSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Campground> GetAllCampgrounds(int parkId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @userInputParkId",conn);
                    cmd.Parameters.AddWithValue("@userInputParkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Campground c = new Campground();
                        c.Campground_id = Convert.ToInt32(reader["campground_Id"]);
                        c.Park_id = Convert.ToInt32(reader["park_Id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.Open_from_mm = Convert.ToInt32(reader["open_from_mm"]);
                        c.Open_to_mm = Convert.ToInt32(reader["open_to_mm"]);
                        c.Daily_fee = Convert.ToInt32(reader["daily_fee"]);

                        output.Add(c);
                       
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return output;
        }
    }
}
