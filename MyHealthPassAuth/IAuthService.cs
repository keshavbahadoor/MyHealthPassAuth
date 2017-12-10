using MyHealthPassAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using the provided username, password. 
        /// Returns a user object. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userAgent"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        User Login(string username, string password, string userAgent, string requestData);

        /// <summary>
        /// Performs a password check using password policy requirements deletgated to a 
        /// specific location. 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CheckPassword(string password, Location location);

        string EncryptPassword(string password);

        string DecryptPassword(string password);
    }
}
