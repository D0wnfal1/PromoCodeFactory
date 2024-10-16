using HotChocolate;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.GraphQL
{
	public class CustomerQueries
	{
		public Task<IEnumerable<CustomerShortResponse>> GetCustomers([Service] Domain.ICustomer customers)
		{
			return customers.GetAllAsync();
		}

		public Task<CustomerResponse> GetCustomer([Service] Domain.ICustomer customers, Guid id)
		{
			return customers.GetByIdAsync(id);
		}

		public async Task<int> AddCustomer([Service] Domain.ICustomer customers, CreateOrEditCustomerRequest request)
		{
			await customers.AddAsync(request);
			return 200;
		}

		public async Task<int> EditCustomer([Service] Domain.ICustomer customers, Guid id, CreateOrEditCustomerRequest request)
		{
			await customers.UpdateAsync(id, request);
			return 200;
		}

		public async Task<int> DeleteCustomer([Service] Domain.ICustomer customers, Guid id)
		{
			await customers.DeleteAsync(id);
			return 200;
		}
	}
}
