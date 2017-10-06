using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;
        private decimal dailyFee;

        public ReservationSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int MakeReservations(int SiteId, string Name, DateTime FromDate, DateTime ToDate, DateTime CreateDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("insert into reservation values (@siteId, @name, @fromDate, @toDate, @createDate)", conn);
                    cmd.Parameters.AddWithValue("@siteId", SiteId);
                    cmd.Parameters.AddWithValue("@name", Name);
                    cmd.Parameters.AddWithValue("@fromDate", FromDate);
                    cmd.Parameters.AddWithValue("@toDate", ToDate);
                    cmd.Parameters.AddWithValue("@createDate", CreateDate);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("SELECT Max(reservation.reservation_id) from reservation;", conn);
                    int reservationConformationId = (int)(cmd.ExecuteScalar());

                    return reservationConformationId;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public decimal CostOfCampground(int campgroundId)
        {
            

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();

                    SqlCommand command = new SqlCommand("SELECT campground.daily_fee FROM campground WHERE campground_id = @userCampId", connect);
                    command.Parameters.AddWithValue("@userCampId", campgroundId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dailyFee = Convert.ToDecimal(reader["daily_fee"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return dailyFee;
        }
    }
}
