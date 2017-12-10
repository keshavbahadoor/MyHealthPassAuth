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
    public class PasswordLengthMinRuleTest
    {
        private AuthorizationConfig config;

        [TestInitialize]
        public void Initialize()
        {
            config = new AuthorizationConfig
            {
                AuthorizationConfigID = 1,
                PasswordLengthMax = 10,
                PasswordLengthMin = 5,
                PasswordAllowedUppercaseCount = 1,
                PasswordAllowedLowercaseCount = 1,
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
        public void TestPasswordShorterThanMin()
        {
            PasswordLengthMinRule rule = new PasswordLengthMinRule();
            Message result = rule.EvaluateRule("keshav", "123", config);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Password length is too short", result.Text);
        }

        [TestMethod]
        public void TestPasswordEqualLength()
        {
            PasswordLengthMinRule rule = new PasswordLengthMinRule();
            Message result = rule.EvaluateRule("keshav", "12345", config);

            Assert.AreEqual(MessageResult.SUCCESS, result.Result);
        }

        [TestMethod]
        public void TestPasswordLongerThanMin()
        {
            PasswordLengthMinRule rule = new PasswordLengthMinRule();
            Message result = rule.EvaluateRule("keshav", "qwerty1234", config);

            Assert.AreEqual(MessageResult.SUCCESS, result.Result);
        }
    }
}
