using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

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
        public static IEnumerable<Partner> Partners => new List<Partner>()
        {
            new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "SuperToys",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                        CreateDate = new DateTime(2020,07,9),
                        EndDate = new DateTime(2020,10,9),
                        Limit = 100
                    }
                }
            },
            new Partner()
            {
                Id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319"),
                Name = "Cats",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("c9bef066-3c5a-4e5d-9cff-bd54479f075e"),
                        CreateDate = new DateTime(2020,05,3),
                        EndDate = new DateTime(2020,10,15),
                        CancelDate = new DateTime(2020,06,16),
                        Limit = 1000
                    },
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("0e94624b-1ff9-430e-ba8d-ef1e3b77f2d5"),
                        CreateDate = new DateTime(2020,05,3),
                        EndDate = new DateTime(2020,10,15),
                        Limit = 100
                    },
                }
            },
            new Partner()
            {
                Id = Guid.Parse("0da65561-cf56-4942-bff2-22f50cf70d43"),
                Name = "The fish of your dreams",
                IsActive = false,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("0691bb24-5fd9-4a52-a11c-34bb8bc9364e"),
                        CreateDate = new DateTime(2020,07,3),
                        EndDate = new DateTime(2020,9,9),
                        Limit = 100
                    }
                }
            },
        };
    }
}
