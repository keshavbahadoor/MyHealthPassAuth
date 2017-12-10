using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.RuleEngine.Interfaces
{
    public interface IAuthRule
    {
        /// <summary>
        /// Applies a rule given the username, password and authorization configs. 
        /// Returns a message to the client. 
        /// A message can either contain a SUCCESS or ERROR enumeration value. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        Message ApplyRule(string username, string password, AuthorizationConfig configs); 
    }
}
