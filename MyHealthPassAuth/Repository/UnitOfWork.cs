using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHealthPassAuth.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Reference to database context 
        /// </summary>
        private readonly MainDbContext _dbContext;

        # region Repositories

        public IRepository<User> UserRepository =>
            new GenericRepository<User>(_dbContext);

        public IRepository<Location> LocationRepository =>
            new GenericRepository<Location>(_dbContext);

        public IRepository<AuthenticationLog> AuthLogRepository =>
            new GenericRepository<AuthenticationLog>(_dbContext);

        # endregion
        /// <summary>
        /// Accepts an external database context 
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(MainDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Saves any changes made to domain models to persistence
        /// </summary>
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        
        /// <summary>
        /// Disposes of databse context. 
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Removes all changes made to domain model. 
        /// Changes will be lost, if not persisted. 
        /// </summary>
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
              .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
