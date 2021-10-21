using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectsTests
{
    [TestClass]
   public class DepartmentDAOTests : ProjectTestBase
    {
        [TestMethod]

        public void GetDepartment_Should_ReturnCorrectDepartmentCount()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);
            // Act
            IEnumerable<Department> result = dao.GetDepartments();
            // Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]

        public void AddDepartment_Should_IncreaseDepartmentCount()
        {
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);

            Department department = new Department();
            department.Name = "New Department";


            int id = dao.CreateDepartment(department);

            Assert.IsTrue(id > 1, "Added department is not valid");
            Assert.AreEqual(2, GetRowCount("department"));
        }

        [TestMethod]
        public void UpdateDepartmentShouldReturnNewDepartmentName()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);
            Department department = new Department();
            department.Name = "test";
            department.Id = 1;

            // Act
            bool result = dao.UpdateDepartment(department);
            string expectedName = GetRowName("name", "department");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedName, "test");

        }
    }
}
