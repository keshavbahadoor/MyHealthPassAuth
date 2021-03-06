﻿using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class PasswordLengthMaxRule : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            if (password.Length > configs.PasswordLengthMax)
            {
                return new Message
                {
                    Result = MessageResult.ERROR, 
                    Text = "Password length is too long"
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
