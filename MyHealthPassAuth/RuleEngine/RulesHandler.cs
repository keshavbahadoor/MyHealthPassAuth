using MyHealthPassAuth.Entities;
using MyHealthPassAuth.RuleEngine.Interfaces;
using MyHealthPassAuth.RuleEngine.Rules;
using MyHealthPassAuth.Services;
using MyHealthPassAuth.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.RuleEngine
{
    public class RulesHandler
    {
        /// <summary>
        /// This will contain a list of rule classes to apply to object. 
        /// TODO : Can move this to persistence layer 
        /// </summary>
        private static readonly List<IAuthRule> _loginAuthRules = new List<IAuthRule>
        { 
            new PasswordLengthRule()
        }; 


        /// <summary>
        /// Evaluates all required login rules on the given username and password. 
        /// Note: We are relying on getting the Location ID from the user object in this case. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Message HandleLogin(string username, string password)
        {
            try
            {
                User user = DataService.Instance.FindUserByUsername(username); 
                if (user == null)
                {
                    return new Message
                    {
                        Result = MessageResult.ERROR, 
                        Text = "User does not exist"
                    };
                }
                AuthorizationConfig config = ConfigService.Instance.GetConfigForLocationID(user.LocationID);
                return EvaluateRules(_loginAuthRules, username, password, config); 
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
        private Message EvaluateRules(List<IAuthRule> rules, string username, string password, AuthorizationConfig configs)
        {
            try
            { 
                foreach (IAuthRule rule in rules)
                {
                    Message message = rule.EvaluateRule(username, password, configs);
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
