using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.RuleEngine;
using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth.Services;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.System.Interfaces;
using MyHealthPassAuth.Security;

namespace MyHealthPassAuth
{
    /// <summary>
    /// Entry class for the library. 
    /// Clients will use this class to leverage authentication and authorization functionality 
    /// </summary>
    public class AuthLib : IAuthService
    {
        private ILog Log;

        private MainDbContext _mainDbContext;

        private IPasswordCrypt _passwordCryptStrategy; 

        public AuthLib(DbContextOptions<MainDbContext> options)
        {
            _mainDbContext = new MainDbContext(options); 

            ConfigService.Instance.Initialize(_mainDbContext);
            DataService.Instance.Initialize(_mainDbContext);

            _passwordCryptStrategy = new DummyCrypt(); 
        }

        public AuthLib(DbContextOptions<MainDbContext> options, ILog log)
            :this(options)
        {
            this.Log = log; 
        }

        public Message CheckPassword(string password, Location location)
        {
            RulesHandler handler = new RulesHandler(Log, new RequestData());
            return handler.CheckPasswordAgainstRules(password, location.LocationID);
        }

        public string DecryptPassword(string password)
        {
            return _passwordCryptStrategy.DecryptPassword(password);
        }

        public string EncryptPassword(string password)
        {
            return _passwordCryptStrategy.EncryptPassword(password); 
        }

        public Message Login(string username, string password, string ipAddress, string userAgent, string requestData)
        {
            RulesHandler handler = new RulesHandler(Log, new RequestData
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Request = requestData
            });
            return handler.HandleLogin(username, _passwordCryptStrategy.EncryptPassword( password));
        }
    }
}
