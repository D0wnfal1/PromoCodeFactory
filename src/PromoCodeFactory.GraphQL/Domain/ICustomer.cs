using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.GraphQL.Domain
{
	public interface ICustomer
	{
		Task<IEnumerable<CustomerShortResponse>> GetAllAsync();
		Task<CustomerResponse> GetByIdAsync(Guid id);
		Task AddAsync(CreateOrEditCustomerRequest request);
		Task UpdateAsync(Guid id, CreateOrEditCustomerRequest request);
		Task DeleteAsync(Guid id);
	}
}
