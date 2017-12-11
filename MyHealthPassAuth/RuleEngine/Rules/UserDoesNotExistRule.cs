using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;
using MyHealthPassAuth.System.Interfaces;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class UserDoesNotExistRule : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            if (DataService.Instance.FindUserByUsername(username) == null)
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "User does not exist"
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
