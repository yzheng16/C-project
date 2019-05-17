using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCeBikeLab.DAL;
using TeamCeBikeLab.Entities.POCOs;

namespace TeamCeBikeLab.BLL.CRUD
{
    public class eBikeUserController
    {
        public List<UserProfile> ListUsers()
        {
            using (var context = new eBikeContext())
            {
                var employees = from emp in context.Employees
                                select new UserProfile
                                {
                                    Id = emp.EmployeeID.ToString(),
                                    Name = emp.FirstName,
                                    OtherName = emp.LastName,
                                    UserType = UserType.Employee
                                };
                var customers = from cust in context.Customers
                                select new UserProfile
                                {
                                    Id = cust.CustomerID.ToString(),
                                    Name = cust.FirstName,
                                    OtherName = cust.LastName,
                                    UserType = UserType.Customer
                                };
                var result = employees.Union(customers);
                return result.ToList();
            }
        }
    }
}
