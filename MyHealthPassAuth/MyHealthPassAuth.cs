using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;

namespace MyHealthPassAuth
{
    /// <summary>
    /// Entry class for the library. 
    /// Clients will use this class to leverage authentication and authorization functionality 
    /// </summary>
    public class MyHealthPassAuth : IAuthService
    {
        public bool CheckPassword(string password, Location location)
        {
            throw new NotImplementedException();
        }

        public string DecryptPassword(string password)
        {
            throw new NotImplementedException();
        }

        public string EncryptPassword(string password)
        {
            throw new NotImplementedException();
        }

        public User Login(string username, string password, string userAgent, string requestData)
        {
            throw new NotImplementedException();
        }
    }
}
