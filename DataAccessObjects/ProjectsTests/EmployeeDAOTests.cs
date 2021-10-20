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
    public class EmployeeDAOTests : ProjectTestBase
    {

        [TestMethod]
        public void GetEmployees_Should_ReturnCorrectAmountOfEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);
            
            // Act
            IEnumerable<Employee> result = dao.GetAllEmployees();
            
            // Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]

        public void AssignEmployee_Should_IncreaseEmployeeCountInRelatorTable()
        {
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);

            bool result = dao.AssignEmployeeToProject(1, 2);

            Assert.IsTrue(result);
            Assert.AreEqual(2, GetRowCount("project_employee"));
        }

        [TestMethod]

        public void RemoveEmployee_Should_ReduceEmployeeCountInRelatorTable()
        {
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);

            bool result = dao.RemoveEmployeeFromProject(1, 1);

            Assert.IsTrue(result);
            Assert.AreEqual(0, GetRowCount("project_employee"));
        }

        [TestMethod]
        public void GetEmployeeNotAssignedToProjectShouldReturnCorrectEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            // Act
            ICollection<Employee> employees = dao.GetEmployeesWithoutProjects();

            Employee tester = new Employee();

            foreach (var employee in employees)
            {
                tester.EmployeeId = employee.EmployeeId;
                tester.BirthDate = employee.BirthDate;
                tester.DepartmentId = employee.DepartmentId;
                tester.FirstName = employee.FirstName;
                tester.LastName = employee.LastName;
                tester.HireDate = employee.HireDate;
                tester.JobTitle = employee.JobTitle;
            }

            // Assert
            Assert.AreEqual(2, tester.EmployeeId);
        }

        [TestMethod]
        public void EmployeeSearchShouldReturnEmployee()
        {
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            Assert.Inconclusive();
        }
    }
}
