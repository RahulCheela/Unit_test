using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnitTestingAppInMVC.Models;
using PagedList;
using System.Data;

namespace UnitTestingAppInMVC.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository employeeRepository;

        public EmployeeController() : this(new EmployeeRepository()) { }

        public EmployeeController(IEmployeeRepository repository)
        {
            employeeRepository = repository;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["ControllerName"] = this.ToString();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Emp_ID" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var employees = from s in employeeRepository.GetAllEmployee()
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Emp ID":
                    employees = employees.OrderByDescending(s => s.Emp_ID);
                    break;
                case "Name":
                    employees = employees.OrderBy(s => s.Name);
                    break;
                case "State":
                    employees = employees.OrderByDescending(s => s.State);
                    break;
                case "Country":
                    employees = employees.OrderByDescending(s => s.Country);
                    break;
                default:
                    employees = employees.OrderBy(s => s.Emp_ID);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Index", employees.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Employee/Details/5

        public ViewResult Details(int id)
        {
            Employee emp = employeeRepository.GetEmployeeByID(id);
            return View(emp);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View("Create");
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employeeRepository.InsertEmployee(emp);
                    employeeRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Some Error Occured.");
            }
            return View("Create", emp);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id)
        {
            Employee emp = employeeRepository.GetEmployeeByID(id);
            return View(emp);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employeeRepository.UpdateEmployee(emp);
                    employeeRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Some error Occured.");
            }
            return View(emp);
        }

        //
        // GET: /employee/Delete/5

        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Some Error Occured.";
            }
            Employee emp = employeeRepository.GetEmployeeByID(id);
            return View(emp);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Employee emp = employeeRepository.GetEmployeeByID(id);
                employeeRepository.DeleteEmployee(id);
                employeeRepository.Save();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            employeeRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
