using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHealthPassAuth.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Reference to database context for this project. 
        /// </summary>
        private readonly MainDbContext _dbContext;

        private DbSet<T> _dbSet => _dbContext.Set<T>();
        public IQueryable<T> Entities => _dbSet;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="dbContext">Databse context to be used.</param>
        public GenericRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Removes an entity 
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Adds a specified generic entity 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            _dbSet.Add(entity);            
        }
    }
}
