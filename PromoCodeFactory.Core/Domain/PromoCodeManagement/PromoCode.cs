using PromoCodeFactory.Core.Domain.Administation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode : BaseEntity
    {
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public DateTime BeginName { get; set; }
        public DateTime EndDate { get; set; }
        public string PartnerName { get; set; }
        public Employee PartnerManager { get; set; }
        public Preference Preference { get; set; }
    }
}
