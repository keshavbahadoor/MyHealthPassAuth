using MyHealthPassAuth.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MyHealthPassAuth.Entities;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;


namespace MyHealthPassAuth.RuleEngine.Rules
{
    public class UserAccountLockRule : AbstractRule
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
             
            if (user.FailedLoginAttempts >= configs.LoginAccountLockAttempts)
            {
                DataService.Instance.LockUserAccount(user);
            }

            if (user.AccountLocked == (int)AccountLockedEnum.LOCKED)
            {
                return new Message
                {
                    Result = MessageResult.ERROR,
                    Text = "Account Locked. Too many login attempts."
                };
            }
            return _defaultSuccessMessage;
        }
    }
}
