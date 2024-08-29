using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly PromoCodeFactoryDataContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public EfRepository(PromoCodeFactoryDataContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetByCondition(Func<T, bool> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate).AsEnumerable());
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public PromoCodeFactoryDataContext GetDbContext()
    {
        return _dbContext;
    }
    public async Task<IEnumerable<CustomerPreference>> GetCustomerPreferencesWithCustomersAsync(Guid preferenceId)
    {
        return await _dbContext.CustomerPreferences
            .Include(cp => cp.Customer)
            .Where(cp => cp.PreferenceId == preferenceId)
            .ToListAsync();
    }
}
