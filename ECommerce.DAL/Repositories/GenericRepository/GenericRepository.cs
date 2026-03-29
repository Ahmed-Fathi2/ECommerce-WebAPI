using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DAL.Repositories.GenericRepository
{
    public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext = dbContext;


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {

            return await _dbContext.Set<T>().FindAsync(id);
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

      
    }
}

