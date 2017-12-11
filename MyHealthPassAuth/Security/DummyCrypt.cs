using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.Security
{
    /// <summary>
    /// does not perform any encryption or decryption. 
    /// </summary>
    public class DummyCrypt : IPasswordCrypt
    {
        public string DecryptPassword(string encryptedPassword)
        {
            return encryptedPassword; 
        }

        public string EncryptPassword(string password)
        {
            return password;
        }
    }
}
