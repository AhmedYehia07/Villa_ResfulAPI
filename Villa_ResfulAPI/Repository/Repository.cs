﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
            this.dbset = dbContext.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbset.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbset;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}