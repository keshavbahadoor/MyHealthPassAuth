using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.System.Interfaces;

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

        protected ILog _log;
        public ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public AbstractRule()
        {

        }

        public AbstractRule(ILog logger)
        {
            this._log = logger; 
        }

        /// <summary>
        /// Applys a rule given username, password, and supplied configs. 
        /// uses hook method for operation 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public Message EvaluateRule(string username, string password, AuthorizationConfig configs, User user)
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

                Message message = this.EvaluateRuleHook(username, password, configs, user);
                LogMessage(message); 
                return message; 
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
        /// Logs the message returned 
        /// </summary>
        /// <param name="message"></param>
        private void LogMessage(Message message)
        {
            if (_log != null)
            {
                _log.LogMessage(this.GetType().Name + " : " + message.Text); 
            }
        }

        /// <summary>
        /// Child classes will handle rule evaluation here. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public abstract Message EvaluateRuleHook(string username, string password, AuthorizationConfig config, User user);
    }
}
