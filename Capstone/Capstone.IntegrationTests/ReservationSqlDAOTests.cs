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
        [TestMethod]
        public void ListAvailableReservationsShouldDisplayReservationAvailability()
        {
            // Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(this.ConnectionString);
            //Act
            IEnumerable<Spaces> result =dao.GetAvailableReservations(DateTime.Parse("10/21/2021"), DateTime.Parse("10/26/2021"), 5, 10, 1);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
         }

    }

}
