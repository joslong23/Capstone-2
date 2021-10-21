using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTests
{
    [TestClass]
    public class ProjectDAOTests : ProjectTestBase
    {
        [TestMethod]
        public void GetProjects_Should_ReturnCorrectProjectCount()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            
            // Act
            IEnumerable<Project> result = dao.GetAllProjects();
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]

        public void CreateProjects_Should_IncreaseProjectCount()
        {
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);

            Project project = new Project();
            project.Name = "New Project";
            project.StartDate = System.DateTime.Today;
            project.EndDate = System.DateTime.Now;

            int id = dao.CreateProject(project);

            Assert.IsTrue(id > 1, "Added project is not valid");
            Assert.AreEqual(2, GetRowCount("project"));
        }
    }
}
