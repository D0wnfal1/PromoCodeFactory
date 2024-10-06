using PromoCodeFactory.Core.Domain.Administation;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode : BaseEntity
    {
        public string Code { get; set; }
        public string ServiceName { get; set; }

        private DateTime _beginDate;
        public DateTime BeginDate
        {
            get => _beginDate;
            set => _beginDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public string PartnerName { get; set; }

        public Guid PartnerManagerId { get; set; }
        public Employee PartnerManager { get; set; }

        public Guid PreferenceId { get; set; }
        public Preference Preference { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
