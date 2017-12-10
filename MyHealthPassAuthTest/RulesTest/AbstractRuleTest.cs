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
    public class AbstractRuleTest
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
        public void TestNullUsername()
        {
            AbstractRuleImpl rule = new AbstractRuleImpl();
            Message result = rule.ApplyRule(null, "1234", config);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Error: username null", result.Text);
        }

        [TestMethod]
        public void TestNullPassword()
        {
            AbstractRuleImpl rule = new AbstractRuleImpl();
            Message result = rule.ApplyRule("keshav", null, config);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Error: password null", result.Text);
        }

        [TestMethod]
        public void TestNullConfigs()
        {
            AbstractRuleImpl rule = new AbstractRuleImpl();
            Message result = rule.ApplyRule("keshav", "1234", null);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Error: configs null", result.Text);
        }
    }
}
