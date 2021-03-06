﻿using Microsoft.EntityFrameworkCore;
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
    public class BruteForceIdentificationRuleTest : AbstractRepositoryTest
    {
        private AuthorizationConfig config;
        private RequestData requestData;
        private User user; 

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

            requestData = new RequestData
            {
                IpAddress = "192.161.1.1", 
                UserAgent = "user.agent", 
                Request = "some.request.cookies"
            };

            user = new User
            {
                UserID = 1, 
                Username = "keshav",
                Password = "pass",
                AccountLocked = (int) AccountLockedEnum.NORMAL,
                FailedLoginAttempts = 1, 
                FailedLoginDateTime = DateTime.Now
            }; 
             
            DataService.Instance.Initialize(this.testDbContext);
        }

        [TestMethod]
        public void TestNoBruteForceBlocking()
        {
            // Add user to db 
            var unitOfWork = new UnitOfWork(testDbContext);
            unitOfWork.UserRepository.Add(user);
            unitOfWork.SaveChanges();

            AddManyAuthenticationLogs(5);

            BruteForceIdentificationRule rule = new BruteForceIdentificationRule(requestData);
            Message result = rule.EvaluateRule(user.Username, user.Password, config, testDbContext.Users.Single());
            
            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(MessageResult.SUCCESS, result.Result);
                Assert.AreEqual(0, context.BlackListLogs.Count());
            }
        }

        [TestMethod]
        public void TestIdentifiedBruteForceAttempt()
        {
            // Add user to db  
            var unitOfWork = new UnitOfWork(testDbContext);
            unitOfWork.UserRepository.Add(user);
            unitOfWork.SaveChanges();

            AddManyAuthenticationLogs(14);

            BruteForceIdentificationRule rule = new BruteForceIdentificationRule(requestData);
            Message result = rule.EvaluateRule(user.Username, user.Password, config, testDbContext.Users.Single());

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(MessageResult.ERROR, result.Result);
                Assert.AreEqual("Too many login attempts from your IP.", result.Text);
                Assert.AreEqual(1, context.BlackListLogs.Count());
            }
        }

        private void AddManyAuthenticationLogs(int amount)
        {
            for (int i = 1; i <= amount; i++)
            {
                TestUtils.AddAuthenticationLog(testDbContext,
                    i,
                    requestData.IpAddress,
                    requestData.Request,
                    requestData.UserAgent,
                    DateTime.Now.AddMinutes(-1),
                    "Error fail");
            }
        }
    }
}
