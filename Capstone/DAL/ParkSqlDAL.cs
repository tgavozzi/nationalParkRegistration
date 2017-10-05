using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    class ParkSqlDAL
    {
        private string connectionString;
        public List<Park> output = new List<Park>();

        public ParkSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
      
        public List<Park> GetAllParks()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park ORDER BY name", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Park p = new Park();
                        p.Park_id = Convert.ToInt32(reader["park_Id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.Establish_date = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Vistitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);

                        output.Add(p);
                        
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }

            return output;
        }
    }
}
