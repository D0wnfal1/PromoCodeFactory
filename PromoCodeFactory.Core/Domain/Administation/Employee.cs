using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Domain.Administation
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public List<EmployeeRole> EmployeeRoles { get; set; }
        public int AppliedPromocodesCount { get; set; }
    }
}
