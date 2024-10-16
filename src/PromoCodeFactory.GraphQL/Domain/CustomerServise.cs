using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.GraphQL.Domain
{
	public class CustomerService : ICustomer
	{
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Preference> _preferenceRepository;
        public CustomerService(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
			_preferenceRepository = preferenceRepository;
        }
        public async Task AddAsync(CreateOrEditCustomerRequest request)
		{
			var preferences = await _preferenceRepository.GetByCondition(x => request.PreferenceIds.Contains(x.Id));

			var customer = new Customer()
			{
				Id = Guid.NewGuid(),
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
			};

			await _customerRepository.CreateAsync(customer);
			await _customerRepository.SaveChangesAsync();

			var customerPreferences = preferences.Select(preference => new CustomerPreference
			{
				CustomerId = customer.Id,
				PreferenceId = preference.Id,
			}).ToList();

			foreach (var customerPreference in customerPreferences)
			{
				customer.CustomerPreferences.Add(customerPreference);
			}
			await _customerRepository.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid id)
		{
			var customer = await _customerRepository.GetByIdAsync(id);

			await _customerRepository.DeleteAsync(customer);
		}

		public async Task<IEnumerable<CustomerShortResponse>> GetAllAsync()
		{
			var customers =  await _customerRepository.GetAllAsync();

			var customer = customers.Select(x => new CustomerShortResponse()
			{
				Email = x.Email,
				FirstName = x.FirstName,
				LastName = x.LastName,
				Id = x.Id,
			});

			return customer.ToList();
		}

		public async Task<CustomerResponse> GetByIdAsync(Guid id)
		{
			var customer = await _customerRepository.GetByIdAsync(id);
											//.Include(c => c.CustomerPreferences)
											//.ThenInclude(cp => cp.Preference)
											//.Include(c => c.PromoCodes)
											//.FirstOrDefaultAsync(c => c.Id == id);


			var customerModel = new CustomerResponse()
			{
				Id = customer.Id,
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Email = customer.Email,
				Preferences = customer.CustomerPreferences
										.Select(cp => new PreferenceResponse()
										{
											Id = cp.Preference.Id,
											Name = cp.Preference.Name,
										})
										.ToList(),
				PromoCodes = customer.PromoCodes
									  .Select(cp => new PromoCodeShortResponse()
									  {
										  Id = cp.Id,
										  Code = cp.Code,
										  ServiceInfo = cp.ServiceName,
										  BeginDate = cp.BeginDate,
										  EndDate = cp.EndDate,
										  PartnerName = cp.PartnerName
									  }).ToList(),
			};

			return customerModel;
		}

		public async Task UpdateAsync(Guid id, CreateOrEditCustomerRequest request)
		{
			var customer = await _customerRepository.GetByIdAsync(id);


			var preferences = (await _preferenceRepository.GetByCondition(x => request.PreferenceIds.Contains(x.Id))).ToList();


			customer.FirstName = request.FirstName;
			customer.LastName = request.LastName;
			customer.Email = request.Email;

			var existingPreferences = (await _preferenceRepository.GetByCondition(cp => cp.Id == id)).ToList();

			foreach (var existingPreference in existingPreferences)
			{
				await _preferenceRepository.DeleteAsync(existingPreference);
			}

			var newCustomerPreferences = preferences.Select(preference => new CustomerPreference
			{
				CustomerId = id,
				PreferenceId = preference.Id,
			}).ToList();

			foreach (var customerPreference in newCustomerPreferences)
			{
				var preference = new Preference()
				{
					Id = customerPreference.Id,
					 Description = customerPreference.Preference.Description,
					 Name = customerPreference.Preference.Name,
				};
				await _preferenceRepository.UpdateAsync(preference);
			}
		}
	}
}
