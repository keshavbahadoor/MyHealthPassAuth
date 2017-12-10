using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class PasswordLengthMinRule : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs)
        {
            if (password.Length < configs.PasswordLengthMin)
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "Password length is too short"
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
