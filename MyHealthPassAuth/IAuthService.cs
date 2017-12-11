using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using the provided username, password.   
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userAgent"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        Message Login(string username, string password, string ipAddress, string userAgent, string requestData);

        /// <summary>
        /// Performs a password check using password policy requirements deletgated to a 
        /// specific location. 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        Message CheckPassword(string password, Location location);

        string EncryptPassword(string password);

        string DecryptPassword(string password);
    }
}
