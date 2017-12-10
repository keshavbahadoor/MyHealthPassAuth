using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class PasswordMatchRule : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            if (user == null)
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "User object not passed"
                };
            }

            // TODO : use decryption here. 
            
            if (!password.Equals(user.Password))
            {
                DataService.Instance.IncrementUserFailedLoginAttempts(user); 
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "Incorrect Password"
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
