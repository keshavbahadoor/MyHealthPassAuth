using MyHealthPassAuth.Entities;
using MyHealthPassAuth.RuleEngine.Interfaces;
using MyHealthPassAuth.RuleEngine.Rules;
using MyHealthPassAuth.Services;
using MyHealthPassAuth.System;
using MyHealthPassAuth.System.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.RuleEngine
{
    public class RulesHandler
    {
        /// <summary>
        /// This will contain a list of rule classes to apply during password check events.  
        /// </summary>
        private List<IAuthRule> _passwordCheckRules = new List<IAuthRule>();  

        /// <summary>
        /// This will contain a list of rules to apply during login events.   
        /// </summary>
        private List<IAuthRule> _loginRules = new List<IAuthRule>();  

        private RequestData requestData;

        private ILog Logger; 

        public RulesHandler(RequestData data)
        {
            this.requestData = data;
            CompileRules();
        }

        public RulesHandler(ILog logger, RequestData data)
            : this(data)
        {
            this.Logger = logger;
            AddLogsToRules();
        }

        /// <summary>
        /// Builds list of rules. 
        /// TODO : Caon move rules to persistence layer and instantiate via ClassName. 
        /// </summary>
        private void CompileRules()
        {
            _passwordCheckRules.Add(new PasswordLengthMinRule());
            _passwordCheckRules.Add(new PasswordLengthMaxRule());
            _passwordCheckRules.Add(new PasswordAllowedLowercaseRule());            

            _loginRules.Add(new CheckBlackListRule(requestData));
            _loginRules.Add(new BruteForceIdentificationRule(requestData));
            _loginRules.Add(new UserDoesNotExistRule());
            _loginRules.Add(new UserAccountLockRule());
            _loginRules.Add(new PasswordMatchRule());           
        }

        /// <summary>
        /// Adds log objects to rules 
        /// </summary>
        private void AddLogsToRules()
        { 
            foreach (AbstractRule rule in _passwordCheckRules)
            {
                rule.Log = this.Logger;
            }
            foreach (AbstractRule rule in _loginRules)
            {
                rule.Log = this.Logger;
            }
        }

        /// <summary>
        /// Performs password validation checks using rules 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public Message CheckPasswordAgainstRules(string password, int locationID)
        {
            try
            {
                AuthorizationConfig config = ConfigService.Instance.GetConfigForLocationID(locationID);
                return EvaluateRules(_passwordCheckRules, "", password, config, null);
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
        /// Evaluates all required login rules on the given username and password. 
        /// User login attempts are resetted if rules evaluation returns as successful. 
        /// Note: We are relying on getting the Location ID from the user object in this case. 
        /// </summary> 
        /// <returns></returns>
        public Message HandleLogin(string username, string password)
        {
            try
            {
                User user = DataService.Instance.FindUserByUsername(username); 
                if (user == null)
                { 
                    user = new User
                    {
                        LocationID = 1
                    }; 
                }
                AuthorizationConfig config = ConfigService.Instance.GetConfigForLocationID(user.LocationID);
                Message message = EvaluateRules(_loginRules, username, password, config, user); 
                if (message.Result == MessageResult.SUCCESS)
                {
                    DataService.Instance.ResetLoginAttempts(user);
                }
                DataService.Instance.AddAuthenticationLog(requestData.IpAddress, 
                    requestData.Request, requestData.UserAgent, message.Text);
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
        /// Iterates through a list of rules and evaluate each one. 
        /// If an error occurs, the operation is broken and the Message object returned. 
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        private Message EvaluateRules(List<IAuthRule> rules, string username, string password, AuthorizationConfig configs, User user)
        {
            try
            { 
                foreach (IAuthRule rule in rules)
                {
                    Message message = rule.EvaluateRule(username, password, configs, user);
                    if (message.Result == MessageResult.ERROR)
                    {
                        return message; 
                    }
                }
                return new Message
                {
                    Result = MessageResult.SUCCESS,
                    Text = "success"
                };
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
    }
}
