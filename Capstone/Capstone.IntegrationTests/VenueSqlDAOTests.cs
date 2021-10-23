using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueSqlDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetAllVenuesShouldReturnCorrectVenueCount()
        {
            // Arrange
            VenueSqlDAO dao = new VenueSqlDAO(this.ConnectionString);
            // Act
            IEnumerable<Venue> result = dao.GetAllVenues();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

        }
        [TestMethod]
        public void DisplayVenueDetailsDisplaysCorrectVenue()
        {
            //Arrange
            VenueSqlDAO dao = new VenueSqlDAO(this.ConnectionString);
            //Act
            IEnumerable<Venue> result = dao.GetVenueDetails();
            Venue test = new Venue();
            foreach(var venue in result)
            {
                test.VenueId = venue.VenueId;
                break;
            }

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, test.VenueId);
        }

    }
}
