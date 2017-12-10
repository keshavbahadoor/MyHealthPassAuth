using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
using MyHealthPassAuth.RuleEngine.Rules;
using MyHealthPassAuth.System;

namespace MyHealthPassAuthTest.RulesTest
{
    [TestClass]
    public class PasswordAllowedLowercaseRuleTest
    {
        private AuthorizationConfig config;

        [TestInitialize]
        public void Initialize()
        {
            config = new AuthorizationConfig
            {
                AuthorizationConfigID = 1,
                PasswordLengthMax = 10,
                PasswordLengthMin = 1,
                PasswordAllowedUppercaseCount = 2,
                PasswordAllowedLowercaseCount = 2,
                PasswordAllowedDigitCount = 1,
                PasswordAllowedSpecialCharCount = 1,
                LoginAccountLockAttempts = 3,
                BruteForceBlockAttempts = 13,
                BruteForceIdentificationSeconds = 600,
                BruteForceBlockSeconds = 600,
                LocationID = 1
            };
        }

        [TestMethod]
        public void TestNotEnoughLowercaseChars()
        {
            PasswordAllowedLowercaseRule rule = new PasswordAllowedLowercaseRule();
            Message result = rule.EvaluateRule("keshav", "QWeTY123", config);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Password must contain 2 lowercase characters", result.Text);
        }

        [TestMethod]
        public void TestEnoughLowercaseChars()
        {
            PasswordAllowedLowercaseRule rule = new PasswordAllowedLowercaseRule();
            Message result = rule.EvaluateRule("keshav", "abceQWERTY123", config);

            Assert.AreEqual(MessageResult.SUCCESS, result.Result);
        }

        
    }
}
