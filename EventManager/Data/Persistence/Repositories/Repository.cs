using EventManager.Data.Core;
using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
        }
        public TEntity Get(int id)
        {
            return context.Set<TEntity>().Find(id);
        }
        public async Task<TEntity> GetAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public bool Exists(int id)
        {
            return context.Set<TEntity>().Any(x => x.Id == id);
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Set<TEntity>().AnyAsync(x => x.Id == id);
        }
        public int Count()
        {
            return context.Set<TEntity>().Count();
        }
        public int CountByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).Count();
        }
        public bool ValueIsInUseByIdForExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).Any();
        }
        public bool Any()
        {
            return context.Set<TEntity>().Any();
        }
        public async Task<bool> AnyAsync()
        {
            return await context.Set<TEntity>().AnyAsync();
        }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void AddAsync(TEntity entity)
        {
            context.Set<TEntity>().AddAsync(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }
        public void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRangeAsync(entities);
        }
        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
        }


    }
}
