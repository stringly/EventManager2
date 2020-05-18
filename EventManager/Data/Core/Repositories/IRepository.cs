using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        bool Any();
        Task<bool> AnyAsync();
        Task<bool> ExistsAsync(int id);
        bool Exists(int id);
        int Count();
        int CountByExpression(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Checks if an Entity exists that matches the Expression parameter.
        /// </summary>
        /// <remarks>
        /// Use this method to quickly check if a Value is in use when adding/updating an Entity.
        /// </remarks>
        /// <example>
        /// <code>
        /// if (Context.Entity.ValueIsInUseByIdForExpression(x => x.Title == someString && x.Id != somethingImEditing.Id);    
        /// </code>
        /// </example>
        /// <param name="predicate">An expression to test.</param>
        /// <returns>Returns true if no Entity was found matching the predicate expression. Otherwise, it will return false.</returns>
        bool ValueIsInUseByIdForExpression(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddAsync(TEntity entity);        
        void AddRange(IEnumerable<TEntity> entities);
        void AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);        
        void RemoveRange(IEnumerable<TEntity> entities);
        
    }
}
