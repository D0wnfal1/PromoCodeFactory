using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        private static readonly Guid AdminId = Guid.NewGuid();
        private static readonly Guid ManagerId = Guid.NewGuid();
        private static readonly Guid AdminRoleId = Guid.NewGuid();
        private static readonly Guid ManagerRoleId = Guid.NewGuid();
        private static readonly Guid PreferenceTheaterId = new Guid("2c07d214-8948-4b3b-8746-d4fa03ce367c");
        private static readonly Guid PreferenceFamilyId = new Guid("dd584031-ec4e-40c4-963e-089a7a45f9cb");
        private static readonly Guid PreferenceChildrenId = new Guid("c83bd9f1-f3e4-44c6-82ae-f49a110991a0");
        private static readonly Guid CustomerId = Guid.NewGuid();

        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee
            {
                Id = AdminId,
                Email = "admin@somemail.com",
                FirstName = "Adam",
                LastName = "Johnson",
                AppliedPromocodesCount = 5
            },
            new Employee
            {
                Id = ManagerId,
                Email = "manager@somemail.com",
                FirstName = "John",
                LastName = "Smith",
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>()
        {
            new Role
            {
                Id = AdminRoleId,
                Name = "Admin",
                Description = "Admin"
            },
            new Role
            {
                Id = ManagerRoleId,
                Name = "PartnerManager",
                Description = "Partner Manager"
            }
        };

        public static IEnumerable<EmployeeRole> EmployeeRoles => new List<EmployeeRole>()
        {
            new EmployeeRole
            {
                EmployeeId = AdminId,
                RoleId = AdminRoleId
            },
            new EmployeeRole
            {
                EmployeeId = ManagerId,
                RoleId = ManagerRoleId
            },
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference
            {
                Id = PreferenceTheaterId,
                Name = "Theater",
                Description = "Promocode for Theater",
            },
            new Preference
            {
                Id = PreferenceFamilyId,
                Name = "Family",
                Description = "Promocode for Family",
            },
            new Preference
            {
                Id = PreferenceChildrenId,
                Name = "Children",
                Description = "Promocode for Children",
            }
        };

        public static IEnumerable<Customer> Customers => new List<Customer>()
        {
            new Customer
            {
                Id = CustomerId,
                Email = "ivan_sergeev@mail.com",
                FirstName = "Simon",
                LastName = "Adams",
                CustomerPreferences = new List<CustomerPreference>
                {
                    new CustomerPreference
                    {
                        PreferenceId = PreferenceTheaterId
                    },
                    new CustomerPreference
                    {
                        PreferenceId = PreferenceFamilyId
                    },
                },
                PromoCodes = new List<PromoCode>
                {
                    new PromoCode
                    {
                        Code = "PROMO123",
                        ServiceName = "MovieTickets",
                        BeginDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(20),
                        PartnerName = "CinemaX",
                        PartnerManagerId = ManagerId,
                        PreferenceId = PreferenceTheaterId
                    },
                    new PromoCode
                    {
                        Code = "PROMO456",
                        ServiceName = "FamilyDinner",
                        BeginDate = DateTime.Now.AddDays(-5),
                        EndDate = DateTime.Now.AddDays(15),
                        PartnerName = "FamilyRestaurant",
                        PartnerManagerId = AdminId,
                        PreferenceId = PreferenceFamilyId
                    },
                },
            }
        };
    }
}
