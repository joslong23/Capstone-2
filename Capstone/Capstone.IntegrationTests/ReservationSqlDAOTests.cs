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

            // Assert
          
        }
        [TestMethod]
        public void ListAvailableReservationsShouldDisplayReservationAvailability()
        {
            // Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(this.ConnectionString);
            //Act
            IEnumerable<Reservation> result = (IEnumerable<Reservation>)dao.GetAvailableReservations(DateTime.Parse("10/15/2021"), DateTime.Parse("10/20/2021"), 5, 10, 1);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
         }

    }

}
