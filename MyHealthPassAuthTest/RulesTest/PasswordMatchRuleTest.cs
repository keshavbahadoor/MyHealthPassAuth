using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
using MyHealthPassAuth.RuleEngine.Rules;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;

namespace MyHealthPassAuthTest.RulesTest
{
    [TestClass]
    public class PasswordMatchRuleTest : AbstractRepositoryTest
    {
        private AuthorizationConfig config; 

        [TestInitialize]
        public void TestInitialize()
        {
            Initialize(); 

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

            TestUtils.AddUserToRepository(testDbContext,
                1,
                "keshav",
                "password123",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));

            DataService.Instance.Initialize(this.testDbContext);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            TearDown(); 
        }

        [TestMethod]
        public void TestPasswordMatchError()
        {
            PasswordMatchRule rule = new PasswordMatchRule();

            Message result = rule.EvaluateRule("keshav", "qwerty123", config, testDbContext.Users.Single());            

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(MessageResult.ERROR, result.Result);
                Assert.AreEqual("Incorrect Password", result.Text);
                Assert.AreEqual(1, context.Users.Single().FailedLoginAttempts); 
            }
        }

        [TestMethod]
        public void TestPasswordMatchErrorMultiple()
        {
            PasswordMatchRule rule = new PasswordMatchRule();

            // attempt 1 
            Message result = rule.EvaluateRule("keshav", "qwerty123", config, testDbContext.Users.Single());

            // attempt 2 
            result = rule.EvaluateRule("keshav", "zxcvbn", config, testDbContext.Users.Single());

            // attempt 3 
            result = rule.EvaluateRule("keshav", "pokemon123", config, testDbContext.Users.Single());

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(MessageResult.ERROR, result.Result);
                Assert.AreEqual("Incorrect Password", result.Text);
                Assert.AreEqual(3, context.Users.Single().FailedLoginAttempts);
            }
        }

        [TestMethod]
        public void TestPasswordMatchSuccess()
        {
            PasswordMatchRule rule = new PasswordMatchRule();

            Message result = rule.EvaluateRule("keshav", "password123", config, testDbContext.Users.Single());

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(MessageResult.SUCCESS, result.Result); 
                Assert.AreEqual(0, context.Users.Single().FailedLoginAttempts);
            }
        }
    }
}
