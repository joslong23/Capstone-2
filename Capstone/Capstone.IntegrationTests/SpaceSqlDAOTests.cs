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
    public class SpaceSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]

        public void GetVenueSpacesShouldReturnAListOfVenues()
        {
            //Arrange
            SpaceSqlDAO dao = new SpaceSqlDAO(this.ConnectionString);
            //Act
            IEnumerable<Spaces> result = dao.GetVenueSpaces(1);
            Spaces test = new Spaces();
            foreach (var space in result)
            {
                test.SpaceId = space.SpaceId;
                break;
            }

            //Assert
            Assert.AreEqual(1, test.SpaceId);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ListAvailableReservationsShouldDisplayReservationAvailability()
        {
            // Arrange
            SpaceSqlDAO dao = new SpaceSqlDAO(this.ConnectionString);
            //Act
            IEnumerable<Spaces> result = dao.GetAvailableReservations(DateTime.Parse("10/21/2021"), DateTime.Parse("10/26/2021"), 5, 10, 1);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
