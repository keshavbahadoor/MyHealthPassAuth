using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using System.Text.RegularExpressions;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class PasswordAllowedLowercaseRule : AbstractRule
    { 
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            if (configs.PasswordAllowedLowercaseCount > Regex.Matches(password, @"\p{Ll}").Count) 
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = string.Format("Password must contain {0} lowercase characters", 
                                        configs.PasswordAllowedLowercaseCount)
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
