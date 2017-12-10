using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class PasswordMatchRule : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
             
            return _defaultSuccessMessage;
        }
    }
}
