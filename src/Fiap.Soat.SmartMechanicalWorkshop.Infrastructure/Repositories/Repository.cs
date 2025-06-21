using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories
{
    public abstract class Repository<T>(DbContext context) where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<Paginate<T>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {


            int totalCount = _dbSet
                    .AsNoTracking().Count();

            if (paginatedRequest.PageNumber == 0)
            {
                return new Paginate<T>(
                    [],
                    totalCount,
                    paginatedRequest.PageSize,
                    paginatedRequest.PageNumber,
                   (int)Math.Ceiling((double)totalCount / paginatedRequest.PageSize)
                );
            }


            List<T> items = await _dbSet
                    .AsNoTracking()
                    .Skip((paginatedRequest.PageNumber - 1) * paginatedRequest.PageSize)
                    .Take(paginatedRequest.PageSize)
                    .ToListAsync(cancellationToken);

            Paginate<T> paginate = new(
                items,
                totalCount,
                paginatedRequest.PageSize,
                paginatedRequest.PageNumber,
                (int)Math.Ceiling((double)totalCount / paginatedRequest.PageSize)
            );

            return paginate;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> insertedEntity = await _dbSet.AddAsync(entity, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return insertedEntity.Entity;
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _ = _dbSet.Update(entity);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _ = _dbSet.Remove(entity);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }
    }
}