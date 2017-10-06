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
    public class ReservationSqlDALTests
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
        public void CostOfStayTests()
        {
            int fakePark;
            int fakeCampground;
            decimal fakeOutput = 2.00M;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Park values ('Big Fake Park', 'FAKE EVERYWHERE', '1919-02-26', 0000, 0, 'THIS IS SO FAKE ITS FUNNY')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select cast(scope_identity() as int);", conn);
                fakePark = Convert.ToInt32(cmd.ExecuteScalar());

                string fakeParkString = $"Insert into Campground values ({fakePark}, 'FAKE CAMPGROUND', 1, 12, 2.00)";

                SqlCommand cmd2 = new SqlCommand(fakeParkString, conn);
                cmd2.ExecuteNonQuery();
                cmd = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeCampground = Convert.ToInt32(cmd.ExecuteScalar());
            }

            ReservationSqlDAL ral = new ReservationSqlDAL(connectionString);
            decimal output = ral.CostOfCampground(fakeCampground);

            Assert.AreEqual(fakeOutput.ToString("C"), output.ToString("C"));
        }

        [TestMethod]
        public void MakeAReservationTest()
        {
            int fakePark;
            int fakeCampground;
            int fakeSite;
            DateTime fakeFrom = Convert.ToDateTime("2500 / 01 / 05");
            DateTime fakeTo = Convert.ToDateTime("2500 / 01 / 07");
            DateTime fakeCreate = Convert.ToDateTime("10 / 06 / 2017");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into Park values ('Big Fake Park', 'FAKE EVERYWHERE', '1919-02-26', 0000, 0, 'THIS IS SO FAKE ITS FUNNY')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select cast(scope_identity() as int);", conn);
                fakePark = Convert.ToInt32(cmd.ExecuteScalar());

                string fakeParkString = $"Insert into Campground values ({fakePark}, 'FAKE CAMPGROUND', 1, 12, 2.00)";

                SqlCommand cmd2 = new SqlCommand(fakeParkString, conn);
                cmd2.ExecuteNonQuery();
                cmd2 = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeCampground = Convert.ToInt32(cmd2.ExecuteScalar());

                string fakeCampSiteString = $"Insert into Site values ({fakeCampground}, 2, 4, 0, 10, 1)";

                SqlCommand cmd3 = new SqlCommand(fakeCampSiteString, conn);
                cmd3.ExecuteNonQuery();
                cmd3 = new SqlCommand("select cast(scope_identity() as int)", conn);
                fakeSite = Convert.ToInt32(cmd3.ExecuteScalar());
            }

            ReservationSqlDAL ral = new ReservationSqlDAL(connectionString);
            int output = ral.MakeReservations(fakeSite, "FAKE HUMAN NAME", fakeFrom , fakeTo, fakeCreate);

            Assert.IsTrue(output > 0);
        }
   
    }
}
