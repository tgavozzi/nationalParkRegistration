using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;
using Capstone.DAL;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class SiteSqlDALTests
    {
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Campground;User ID=te_student;Password=sqlserver1";
        TransactionScope t;

        [TestInitialize]
        public void Initialize()
        {
            t = new TransactionScope();
        }

        [TestCleanup]
        public void CleanUp()
        {
            t.Dispose();
        }

        [TestMethod]
        public void CampsiteAvailability()
        {
            int fakePark;
            int fakeCampground;
            int fakeSite;
            DateTime fakeFrom = Convert.ToDateTime("2020 / 01 / 05");
            DateTime fakeTo = Convert.ToDateTime("2020 / 01 / 07");
            DateTime fakeCreate = Convert.ToDateTime("10 / 06 / 2017");
            DateTime fakeReservationCreate = DateTime.Now;
            int fakeOpenMonth = 1;
            int fakeCloseMonth = 11;
            int fakeReservation;
            decimal fakeMoney = 2.00M;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Park values ('Big Fake Park', 'FAKE EVERYWHERE', '1919-02-26', 0000, 0, 'THIS IS SO FAKE ITS FUNNY')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select cast(scope_identity() as int);", conn);
                fakePark = Convert.ToInt32(cmd.ExecuteScalar());

                string fakeParkString = $"Insert into Campground values ({fakePark}, 'FAKE CAMPGROUND', {fakeOpenMonth}, {fakeCloseMonth}, {fakeMoney})";

                SqlCommand cmd2 = new SqlCommand(fakeParkString, conn);
                cmd2.ExecuteNonQuery();
                cmd2 = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeCampground = Convert.ToInt32(cmd2.ExecuteScalar());

                string fakeCampSiteString = $"Insert into Site values ({fakeCampground}, 2, 4, 0, 10, 1)";

                SqlCommand cmd3 = new SqlCommand(fakeCampSiteString, conn);
                cmd3.ExecuteNonQuery();
                cmd3 = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeSite = Convert.ToInt32(cmd3.ExecuteScalar());

                string fakeReservationString = $"Insert into Reservation values ({fakeSite}, 'FAKE HUMAN NAME', '2017/09/01', '2017/09/05', '10/06/2017')";

                SqlCommand cmd4 = new SqlCommand(fakeReservationString, conn);
                cmd4.ExecuteNonQuery();
                cmd4 = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeReservation = Convert.ToInt32(cmd4.ExecuteScalar());
                
            }

            SiteSqlDAL sal = new SiteSqlDAL(connectionString);
            List<Site> output = sal.CampsiteAvailability(fakeCampground, fakeFrom, fakeTo);

            Assert.IsFalse(output.Count < 1);
        }

         [TestMethod]
            public void GetAllCampgroundsTest()
            {
                int parkId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO park VALUES ('fake park', 'fake location', '9/9/9999', 99, 99, 'fake description');", conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    parkId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                int campgroundId;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"INSERT INTO campground VALUES ({parkId}, 'fake campground', '1', '2', '99');", conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    campgroundId = Convert.ToInt32(cmd.ExecuteScalar());

                }

                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> campground = dal.GetAllCampgrounds(parkId);
                CollectionAssert.Contains(campground, campgroundId);
        }
    }
}
