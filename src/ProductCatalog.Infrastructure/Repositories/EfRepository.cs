using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _ctx;
    private readonly DbSet<T> _dbSet;

    public EfRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = _ctx.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop == null) throw new InvalidOperationException("Entity must have Id property");
        var entity = await _dbSet.FindAsync(id);
        if (entity != null) { _dbSet.Remove(entity); await _ctx.SaveChangesAsync(); }
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id) as T;

    public async Task<IEnumerable<T>> ListAsync() => await _dbSet.ToListAsync();

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _ctx.SaveChangesAsync();
    }
}
