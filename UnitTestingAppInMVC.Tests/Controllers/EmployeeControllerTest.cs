using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UnitTestingAppInMVC.Controllers;
using UnitTestingAppInMVC.Models;
using UnitTestingAppInMVC.Tests.Models;

namespace UnitTestingAppInMVC.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
    {
        /// <summary>
        /// This method used for index view
        /// </summary>
        [TestMethod]
        public void IndexView()
        {
            var empcontroller = GetEmployeeController(new InMemoryEmployeeRepository());
           
            ViewResult result = empcontroller.Index(null, null, null, null);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        /// <summary>
        /// This method used to get employee controller
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private static EmployeeController GetEmployeeController(IEmployeeRepository emprepository)
        {
            EmployeeController empcontroller = new EmployeeController(emprepository);
            empcontroller.ControllerContext = new ControllerContext()
            {
                Controller = empcontroller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return empcontroller;
        }

        /// <summary>
        ///  This method used to get all employye listing
        /// </summary>
        [TestMethod]
        public void GetAllEmployeeFromRepository()
        {
            // Arrange
            Employee employee1 = GetEmployeeName(1, "Rahul Saxena", "rahulsaxena@live.com", "Software Developer", "Noida", "Uttar Pradesh", "India");
            Employee employee2 = GetEmployeeName(2, "Abhishek Saxena", "abhishek@abhishek.com", "Tester", "Saharanpur", "Uttar Pradesh", "India");
            InMemoryEmployeeRepository emprepository = new InMemoryEmployeeRepository();
            emprepository.InsertEmployee(employee1);
            emprepository.InsertEmployee(employee2);
            var controller = GetEmployeeController(emprepository);
            var result = controller.Index(null, null, null, null);
            var datamodel = (IEnumerable<Employee>)result.ViewData.Model;
            CollectionAssert.Contains(datamodel.ToList(), employee1);
            CollectionAssert.Contains(datamodel.ToList(), employee2);
        }

        /// <summary>
        /// This method used to get emp name
        /// </summary>
        /// <param name="Emp_ID"></param>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="Designation"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="Country"></param>         
        /// <returns></returns>
        Employee GetEmployeeName(int Emp_ID, string Name, string Email, string Designation, string City, string State, string Country)
        {
            return new Employee
            {
                Emp_ID = Emp_ID,
                Name = Name,
                Email = Email,
                Designation = Designation,
                City = City,
                State = State,
                Country = Country
            };
        }

        /// <summary>
        /// This test method used to post employee
        /// </summary>
        [TestMethod]
        public void Create_PostEmployeeInRepository()
        {
            InMemoryEmployeeRepository emprepository = new InMemoryEmployeeRepository();
            EmployeeController empcontroller = GetEmployeeController(emprepository);
            Employee employee = GetEmployeeID();
            empcontroller.Create(employee);
            IEnumerable<Employee> employees = emprepository.GetAllEmployee();
            Assert.IsTrue(employees.Contains(employee));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Employee GetEmployeeID()
        {
            return GetEmployeeName(1, "Rahul Saxena", "rahulsaxena@live.com", "Software Developer", "Noida", "Uttar Pradesh", "India");
        }

        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void Create_PostRedirectOnSuccess()
        {
            EmployeeController controller = GetEmployeeController(new InMemoryEmployeeRepository());
            Employee model = GetEmployeeID();
            var result = (RedirectToRouteResult)controller.Create(model);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void ViewIsNotValid()
        {
            EmployeeController empcontroller = GetEmployeeController(new InMemoryEmployeeRepository());
            empcontroller.ModelState.AddModelError("", "mock error message");
            Employee model = GetEmployeeName(1, "", "", "", "", "", "");
            var result = (ViewResult)empcontroller.Create(model);
            Assert.AreEqual("Create", result.ViewName);
        }
    }

    public class MockHttpContext : HttpContextBase
    {
        private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);

        public override IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                base.User = value;
            }
        }
    }
}
