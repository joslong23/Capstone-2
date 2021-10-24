using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class ReservationSqlDAOTests : IntegrationTestBase
    {

        [TestMethod]

        public void MakeReservationsShouldAddToReservationCount()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(this.ConnectionString);
            //Act
            Reservation results = dao.MakeReservation(DateTime.Parse("10/15/20"), DateTime.Parse("10/20/2021"), 10, 5, 1, "The Saints");
            

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual("The Saints", results.ReservationReservedFor);
            Assert.IsTrue(results.ReservationId > 1);
        }
   
    }

}
