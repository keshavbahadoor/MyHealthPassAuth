﻿using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class AbstractRuleImpl : AbstractRule
    {
        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig config, User user)
        {
            return new Message
            {
                Result = MessageResult.SUCCESS,
                Text = this.GetType().Name
            }; 
        }
    }
}
