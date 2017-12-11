using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;

namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class BruteForceIdentificationRule : AbstractRule
    {
        private RequestData requestData; 

        public BruteForceIdentificationRule(RequestData data)
        {
            this.requestData = data; 
        }

        public override Message EvaluateRuleHook(string username, string password, AuthorizationConfig configs, User user)
        {
            int attemptCount = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                    requestData.IpAddress,
                    requestData.Request,
                    requestData.UserAgent,
                    configs.BruteForceIdentificationSeconds
                ); 

            if (attemptCount >= configs.BruteForceBlockAttempts)
            {
                DataService.Instance.AddOrUpdateBlackListLog(requestData.IpAddress, requestData.UserAgent); 

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
