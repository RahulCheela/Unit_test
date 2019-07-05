using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingAppInMVC.Models;

namespace UnitTestingAppInMVC.Tests.Models
{
    class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _db = new List<Employee>();

        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _db.ToList();
        }

        public Employee GetEmployeeByID(int id)
        {
            return _db.FirstOrDefault(d => d.Emp_ID == id);
        }

        public void InsertEmployee(Employee employeeToCreate)
        {
            

            _db.Add(employeeToCreate);
        }

        public void DeleteEmployee(int id)
        {
            _db.Remove(GetEmployeeByID(id));
        }


        public void UpdateEmployee(Employee employeeToUpdate)
        {

            foreach (Employee employee in _db)
            {
                if (employee.Emp_ID == employeeToUpdate.Emp_ID)
                {
                    _db.Remove(employee);
                    _db.Add(employeeToUpdate);
                    break;
                }
            }
        }

        public int Save()
        {
            return 1;
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {                    
                     //Dispose Object Here
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
