using Microsoft.EntityFrameworkCore;
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
        /// Requires a database context object for usage. 
        /// </summary>
        /// <param name="databaseContextObject"></param>
        public void Initialize(MainDbContext databaseContextObject)
        {
            _dbContext = databaseContextObject;
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

        /// <summary>
        /// Increments the supplied user's login attempts by 1 
        /// </summary>
        /// <param name="user"></param>
        public void IncrementUserFailedLoginAttempts(User user)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }
                user.FailedLoginAttempts += 1;
                user.FailedLoginDateTime = DateTime.Now; 
                _dbUnitOfWork.SaveChanges(); 
            }
            catch (Exception ex)
            {
                // TODO: some logging would be nice.              
            }
        }

        /// <summary>
        /// Sets login attempts for user back to 0 
        /// This is called on successful login
        /// </summary>
        /// <param name="user"></param>
        public void ResetLoginAttempts(User user)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }
                user.FailedLoginAttempts = 0; 
                _dbUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: some logging would be nice.              
            }
        }

        /// <summary>
        /// Locks the user account 
        /// </summary>
        /// <param name="user"></param>
        public void LockUserAccount(User user)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }
                user.AccountLocked = (int)AccountLockedEnum.LOCKED;
                _dbUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: some logging would be nice.              
            }
        }

        /// <summary>
        /// Creates an auth log entry. 
        /// TODO : can be done asynchronously 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="request"></param>
        /// <param name="userAgent"></param>
        public void AddAuthenticationLog(string ipAddress, string request, string userAgent, string resultMessage)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }
                _dbUnitOfWork.AuthLogRepository.Add(new AuthenticationLog
                {
                    IpAddress = ipAddress,
                    RequestData = request,
                    UserAgent = userAgent,
                    InsertDate = DateTime.Now,
                    ResultMessage = resultMessage
                });
                _dbUnitOfWork.SaveChanges(); 
            }
            catch (Exception ex)
            {
                // TODO: some logging would be nice.              
            }
        }

    }
}
