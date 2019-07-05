using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UnitTestingAppInMVC.Models
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {       

        EmployeeManagementEntities context = new EmployeeManagementEntities();        

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Employee.ToList();
        }

        public Employee GetEmployeeByID(int id)
        {
            return context.Employee.Find(id);
        }

        public void InsertEmployee(Employee emp)
        {
            context.Employee.Add(emp);
        }

        public void DeleteEmployee(int emp_ID)
        {
            Employee emp = context.Employee.Find(emp_ID);
            context.Employee.Remove(emp);
        }

        public void UpdateEmployee(Employee emp)
        {
            context.Entry(emp).State = EntityState.Modified;
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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