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
    public class PasswordLengthRuleTest
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
        public void TestPasswordLongerThanLength()
        {
            PasswordLengthRule rule = new PasswordLengthRule();
            Message result = rule.EvaluateRule("keshav", "1234567890awesows", config);

            Assert.AreEqual(MessageResult.ERROR, result.Result);
            Assert.AreEqual("Password length is too short", result.Text);
        }
        
        [TestMethod]
        public void TestPasswordEqualLength()
        {
            PasswordLengthRule rule = new PasswordLengthRule();
            Message result = rule.EvaluateRule("keshav", "1234567890", config);

            Assert.AreEqual(MessageResult.SUCCESS, result.Result); 
        }

        [TestMethod]
        public void TestPasswordShorterThanLength()
        {
            PasswordLengthRule rule = new PasswordLengthRule();
            Message result = rule.EvaluateRule("keshav", "qwerty", config);

            Assert.AreEqual(MessageResult.SUCCESS, result.Result);
        }
    }
}
