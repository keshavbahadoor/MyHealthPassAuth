using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public abstract class AbstractRule : IAuthRule
    {
        /// <summary>
        /// Provides a default message to return on success purposes
        /// </summary>
        protected Message _defaultSuccessMessage = new Message
        {
            Result = MessageResult.SUCCESS,
            Text = "success"
        }; 
        public Message DefaultSuccessMessage
        {
            get { return _defaultSuccessMessage; }
        }

        /// <summary>
        /// Child classes will handle rule 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public abstract Message ApplyRule(string username, string password, AuthorizationConfig configs);
    }
}
