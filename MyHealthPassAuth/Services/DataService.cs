﻿using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyHealthPassAuth.Services
{
    /// <summary>
    /// General data service for the library. This can be refactored into smaller 
    /// components if required. Sticking to this design for now. 
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Local private instance 
        /// </summary>
        private static readonly DataService _instance = new DataService();

        /// <summary>
        /// Database context that exposes repository
        /// </summary>
        private MainDbContext _dbContext;
        public MainDbContext DbContext
        {
            get { return _dbContext; }
            set { _dbContext = value; }
        }

        private UnitOfWork _dbUnitOfWork; 

        private DataService()
        {
            
        }

        /// <summary>
        /// Requires a database context options object for usage. 
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public void Initialize(DbContextOptions<MainDbContext> dbContextOptions)
        {
            _dbContext = new MainDbContext(dbContextOptions);
            _dbUnitOfWork = new UnitOfWork(_dbContext); 
        }

        /// <summary>
        /// Thread safe singleton implementation 
        /// </summary>
        public static DataService Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Locates a user based on the provided username. 
        /// Returns null if a user is not found in persistence layer
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User FindUserByUsername(string username)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized"); 
                }

                return _dbUnitOfWork.UserRepository.Entities
                    .Where(u => u.Username.Equals(username))
                    .FirstOrDefault();                    
            }
            catch(Exception ex)
            {
                // TODO: some logging would be nice. 
                return null; 
            }
        }

    }
}