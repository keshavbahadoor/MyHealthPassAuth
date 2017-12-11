using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.Security
{
    public interface IPasswordCrypt
    {
        /// <summary>
        /// Performs password encryption on given password
        /// Returns encrypted string 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string EncryptPassword(string password);

        /// <summary>
        /// Performs password decryption on given encrypted password
        /// Returns plain text string
        /// </summary>
        /// <param name="encryptedPassword"></param>
        /// <returns></returns>
        string DecryptPassword(string encryptedPassword);
    }
}
