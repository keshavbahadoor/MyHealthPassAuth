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
    public class CheckBlackListRule : AbstractRule
    {
        private RequestData requestData;

        public CheckBlackListRule(RequestData data)
        {
            this.requestData = data;
        } 

        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            if (DataService.Instance.IsClientBlackListedInTheListSeconds(
                    requestData.IpAddress,
                    requestData.UserAgent,
                    configs.BruteForceBlockSeconds
                ))
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "Too many login attempts from your IP."
                };
            }        
            return _defaultSuccessMessage;
        }
    }
}
