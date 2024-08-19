using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Domain.Administation
{
    public class EmployeeRole : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
