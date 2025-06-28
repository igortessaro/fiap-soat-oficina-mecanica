using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public abstract class Repository<T>(DbContext context) : IRepository<T> where T : class
{
    private readonly DbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public Task<Paginate<T>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken) =>
        GetAllAsync(_dbSet, paginatedRequest, cancellationToken);

    public Task<Paginate<T>> GetAllAsync(Expression<Func<T, bool>> predicate, PaginatedRequest paginatedRequest, CancellationToken cancellationToken) =>
        GetAllAsync(_dbSet.Where(predicate), paginatedRequest, cancellationToken);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) =>
        _dbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync(cancellationToken);

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        var insertedEntity = await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return insertedEntity.Entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken)
    {
        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) =>
        _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);

    protected IQueryable<T> Query(bool noTracking = true) => noTracking ? _dbSet.AsQueryable().AsNoTracking() : _dbSet.AsQueryable();

    private async Task<Paginate<T>> GetAllAsync(IQueryable<T> query, PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        int totalCount = await query.AsNoTracking().CountAsync(cancellationToken);
        if (paginatedRequest.PageNumber == 0)
        {
            return new Paginate<T>(
                [],
                totalCount,
                paginatedRequest.PageSize,
                paginatedRequest.PageNumber,
                (int) Math.Ceiling((double) totalCount / paginatedRequest.PageSize)
            );
        }

        var items = await query
            .AsNoTracking()
            .Skip((paginatedRequest.PageNumber - 1) * paginatedRequest.PageSize)
            .Take(paginatedRequest.PageSize)
            .ToListAsync(cancellationToken);

        Paginate<T> paginate = new(
            items,
            totalCount,
            paginatedRequest.PageSize,
            paginatedRequest.PageNumber,
            (int) Math.Ceiling((double) totalCount / paginatedRequest.PageSize)
        );

        return paginate;
    }
}
