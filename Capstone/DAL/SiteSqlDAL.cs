using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;

        public SiteSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Site> CampsiteAvailability(int campgroundId, DateTime fromDate, DateTime toDate)
        {
            List<Site> checkList = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select top 5 * from site where site_id not in (select reservation.site_id from reservation where (reservation.from_date < @userFrom and reservation.to_date > @userTo )or (reservation.from_date > @userFrom and reservation.to_date < @userTo )or (reservation.from_date = @userFrom and reservation.to_date = @userTo)) and site.campground_id = @userCamp", conn);

                    cmd.Parameters.AddWithValue("@userFrom", fromDate);
                    cmd.Parameters.AddWithValue("@userTo", toDate);
                    cmd.Parameters.AddWithValue("@userCamp", campgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.SiteID = Convert.ToInt32(reader["site_id"]);
                        s.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        s.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        s.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);

                        checkList.Add(s);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return checkList;
        }
    }
}
