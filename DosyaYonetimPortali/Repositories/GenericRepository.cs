using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DosyaYonetimPortali.Context;

namespace DosyaYonetimPortali.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter) => await _dbSet.Where(filter).ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;
    }
}