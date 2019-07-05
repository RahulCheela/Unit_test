using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingAppInMVC.Models
{
    public interface IEmployeeRepository : IDisposable
    {
        IEnumerable<Employee> GetAllEmployee();
        Employee GetEmployeeByID(int emp_ID);
        void InsertEmployee(Employee emp);
        void DeleteEmployee(int emp_ID);
        void UpdateEmployee(Employee emp);
        int Save();
    }
}
