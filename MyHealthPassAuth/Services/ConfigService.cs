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
    /// This will enable access to authorization configurations (Example: 
    ///     password length configs, allowed password size, etc.) 
    /// Ideally, this should use some sort of configuration mamangement, 
    /// but the database context will do for now. 
    /// </summary>
    public class ConfigService
    {
        /// <summary>
        /// Local private instance 
        /// </summary>
        private static readonly ConfigService _instance = new ConfigService();

        /// <summary>
        /// List of all configurations available 
        /// </summary>
        private List<AuthorizationConfig> _configs;
        public List<AuthorizationConfig> Configs
        {
            get { return _configs; }            
        }

        /// <summary>
        /// Represents the time that the configurtions was loaded. 
        /// The idea here is to only read configs from database into memory infrequently, and not 
        /// at each request. 
        /// </summary>
        private DateTime _lastConfigListUpdate; 

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

        private ConfigService()
        {
            _configs = new List<AuthorizationConfig>();
        }

        /// <summary>
        /// Thread safe singleton implementation 
        /// </summary>
        public static ConfigService Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Requires a database context options object for usage. 
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public void Initialize(DbContextOptions<MainDbContext> dbContextOptions)
        {
            _dbContext = new MainDbContext(dbContextOptions);
            _dbUnitOfWork = new UnitOfWork(_dbContext);
            LoadConfigurationList(); 
        }

        /// <summary>
        /// Creates a local configuration list from database. 
        /// </summary>
        public void LoadConfigurationList()
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }

                this._configs.Clear(); 
                this._configs = _dbUnitOfWork.AuthConfigRepository.Entities.ToList(); 
                this._lastConfigListUpdate = DateTime.Now;
            }
            catch(Exception ex)
            {
                // TODO : some logging would be nice
            }            
        }

        /// <summary>
        /// Returns a configuration object for a specified location ID.
        /// If a location is not found, returns null. 
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public AuthorizationConfig GetConfigForLocationID(int locationID)
        {
            try
            {
                if (_dbContext == null)
                {
                    throw new Exception("Database context not initialized");
                }

                foreach(AuthorizationConfig config in _configs)
                {
                    if (config.LocationID == locationID)
                        return config; 
                }
                return null; 
            }
            catch(Exception ex)
            {
                // TODO : some logging would be nice
                return null; 
            }
        }
         
    }
}
