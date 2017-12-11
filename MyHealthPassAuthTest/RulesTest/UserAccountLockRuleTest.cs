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
    public class UserAccountLockRuleTest : AbstractRepositoryTest
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

            

            DataService.Instance.Initialize(this.testDbContext);
        }

        [TestMethod]
        public void TestLockUserAfterAttemps()
        {
            var unitOfWork = new UnitOfWork(testDbContext);
            unitOfWork.UserRepository.Add(new User
            {
                UserID = 1,
                Username = "keshav",
                FailedLoginAttempts = 4,                
                LocationID = 1,
                Password = "pass"
            });
            unitOfWork.SaveChanges();

            UserAccountLockRule rule = new UserAccountLockRule();

            Message result = rule.EvaluateRule("keshav", "incorrectpass", config, testDbContext.Users.Single());

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            { 
                Assert.AreEqual((int)AccountLockedEnum.LOCKED, context.Users.Single().AccountLocked);
                Assert.AreEqual(MessageResult.ERROR, result.Result);
                Assert.AreEqual("Account Locked. Too many login attempts.", result.Text);
            }           
        }

        [TestMethod]
        public void TestLockedUserNotAllowed()
        {
            var unitOfWork = new UnitOfWork(testDbContext);
            unitOfWork.UserRepository.Add(new User
            {
                UserID = 1,
                Username = "keshav",
                FailedLoginAttempts = 4,
                LocationID = 1,
                AccountLocked = (int)AccountLockedEnum.LOCKED,
                Password = "pass"
            });
            unitOfWork.SaveChanges();

            UserAccountLockRule rule = new UserAccountLockRule();

            Message result = rule.EvaluateRule("keshav", "incorrectpass", config, testDbContext.Users.Single());

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            { 
                Assert.AreEqual(MessageResult.ERROR, result.Result);
                Assert.AreEqual("Account Locked. Too many login attempts.", result.Text);
            }
        }

        [TestMethod]
        public void TestUserNotLocked()
        {
            TestUtils.AddUserToRepository(testDbContext,
                1,
                "keshav",
                "password123",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));

            UserAccountLockRule rule = new UserAccountLockRule();

            Message result = rule.EvaluateRule("keshav", "password123", config, testDbContext.Users.Single());
            Assert.AreEqual(MessageResult.SUCCESS, result.Result);
        }
    }
}
