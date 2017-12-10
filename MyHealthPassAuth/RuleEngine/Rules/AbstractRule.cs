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
        /// Applys a rule given username, password, and supplied configs. 
        /// uses hook method for operation 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public Message EvaluateRule(string username, string password, AuthorizationConfig configs)
        {
            try
            {
                if (configs == null)
                {
                    return new Message
                    {
                        Result = MessageResult.ERROR,
                        Text = "Error: configs null"
                    };
                }
                if (username == null)
                {
                    return new Message
                    {
                        Result = MessageResult.ERROR,
                        Text = "Error: username null"
                    };
                }
                if (password == null)
                {
                    return new Message
                    {
                        Result = MessageResult.ERROR,
                        Text = "Error: password null"
                    };
                }

                return this.ApplyRuleHook(username, password, configs);
            }
            catch(Exception ex)
            {
                // TODO : Some logging would be nice 

                return new Message
                {
                    Result = MessageResult.ERROR, 
                    Text = "Error: " + ex.Message
                };
            }
        }

        /// <summary>
        /// Child classes will handle rule evaluation here. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public abstract Message ApplyRuleHook(string username, string password, AuthorizationConfig config);
    }
}
