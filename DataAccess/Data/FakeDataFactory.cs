using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Employee> Employees => new List<Employee>()
    {
        new Employee
        {
            Id = Guid.NewGuid(),
            Email = "owner@somemail.com",
            FirstName = "Adam",
            LastName = "Johnson",
            AppliedPromocodesCount = 5
        },
        new Employee
        {
            Id = Guid.NewGuid(),
            Email = "andreev@somemail.com",
            FirstName = "John",
            LastName = "Smith",
            AppliedPromocodesCount = 10
        },
    };

        public static IEnumerable<Role> Roles => new List<Role>()
    {
        new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Description = "Admin"
        },
        new Role
        {
            Id = Guid.NewGuid(),
            Name = "PartnerManager",
            Description = "Partner Manager"
        }
    };

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.NewGuid(),
                Name = "Theater"
            },
            new Preference()
            {
                Id = Guid.NewGuid(),
                Name = "Family"
            },
            new Preference()
            {
                Id = Guid.NewGuid(),
                Name = "Children"
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customerId = Guid.NewGuid();
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = customerId,
                        Email = "ivan_sergeev@mail.com",
                        FirstName = "Simon",
                        LastName = "Adams"
                        //TODO: Add a pre-populated preference list
                    }
                };
                return customers;
            }
        }
    }
}
