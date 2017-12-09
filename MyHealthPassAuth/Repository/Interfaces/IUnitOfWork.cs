﻿using MyHealthPassAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        /// <summary>
        /// Saves all changes to persistence 
        /// </summary>
        void Commit();

        /// <summary>
        /// Discard all changes that has not been committed 
        /// </summary>
        void RejectChanges();

    }
}
