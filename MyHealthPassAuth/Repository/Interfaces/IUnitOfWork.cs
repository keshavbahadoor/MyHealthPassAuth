using MyHealthPassAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Location> LocationRepository { get; }

        IRepository<AuthenticationLog> AuthLogRepository { get; }

        IRepository<AuthorizationConfig> AuthConfigRepository { get; }

        IRepository<BlackListLog> BlackListLogRepository { get; }

        /// <summary>
        /// Saves all changes to persistence 
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Discard all changes that has not been committed 
        /// </summary>
        void RejectChanges();

    }
}
