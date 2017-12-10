using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class AuthConfigRepositoryTest : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
        }

        [TestMethod]
        public void TestAuthConfigInsert()
        {
            AddAuthConfig(this.testDbContext, 1, 15, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1); 

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthorizationConfig.Count());
                Assert.AreEqual(15, context.AuthorizationConfig.Single().PasswordLengthMax);
                Assert.AreEqual(5, context.AuthorizationConfig.Single().PasswordLengthMin);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedUppercaseCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedLowercaseCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedDigitCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedSpecialCharCount);
                Assert.AreEqual(10000, context.AuthorizationConfig.Single().MaxUserSessionSeconds);
                Assert.AreEqual(3, context.AuthorizationConfig.Single().LoginAccountLockAttempts);
                Assert.AreEqual(13, context.AuthorizationConfig.Single().BruteForceBlockAttempts);
                Assert.AreEqual(600, context.AuthorizationConfig.Single().BruteForceIdentificationSeconds);
                Assert.AreEqual(1200, context.AuthorizationConfig.Single().BruteForceBlockSeconds);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().LocationID);
            }
        }

        [TestMethod]
        public void TestAuthConfigUpdate()
        {
            AddAuthConfig(this.testDbContext, 1, 15, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1);

            var unitOfWork = new UnitOfWork(testDbContext);
            var authConfig = unitOfWork.AuthConfigRepository.Entities
                    .First(a => a.AuthorizationConfigID == 1);

            authConfig.BruteForceBlockAttempts = 20;
            authConfig.BruteForceIdentificationSeconds = 1500;
            unitOfWork.SaveChanges();

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthorizationConfig.Count());
                Assert.AreEqual(15, context.AuthorizationConfig.Single().PasswordLengthMax);
                Assert.AreEqual(5, context.AuthorizationConfig.Single().PasswordLengthMin);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedUppercaseCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedLowercaseCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedDigitCount);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().PasswordAllowedSpecialCharCount);
                Assert.AreEqual(10000, context.AuthorizationConfig.Single().MaxUserSessionSeconds);
                Assert.AreEqual(3, context.AuthorizationConfig.Single().LoginAccountLockAttempts);
                Assert.AreEqual(20, context.AuthorizationConfig.Single().BruteForceBlockAttempts);
                Assert.AreEqual(1500, context.AuthorizationConfig.Single().BruteForceIdentificationSeconds);
                Assert.AreEqual(1200, context.AuthorizationConfig.Single().BruteForceBlockSeconds);
                Assert.AreEqual(1, context.AuthorizationConfig.Single().LocationID);
            }
        }


        /// <summary>
        /// Adds a auth config to data context. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="passLengthMax"></param>
        /// <param name="passLengthMin"></param>
        /// <param name="upperCase"></param>
        /// <param name="lowerCase"></param>
        /// <param name="digit"></param>
        /// <param name="specialChars"></param>
        /// <param name="maxUserSessionSec"></param>
        /// <param name="loginAttemptCount"></param>
        /// <param name="bruteForceBlockAttempts"></param>
        /// <param name="bruteForceIdSeconds"></param>
        /// <param name="bruteForceBlockSec"></param>
        /// <param name="locationid"></param>
        private void AddAuthConfig(MainDbContext context, int id, int passLengthMax, int passLengthMin, 
            int upperCase, int lowerCase, int digit, int specialChars, 
            int maxUserSessionSec, int loginAttemptCount, int bruteForceBlockAttempts, 
            int bruteForceIdSeconds, int bruteForceBlockSec, int locationid)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.AuthConfigRepository.Add(new AuthorizationConfig
            {
                AuthorizationConfigID = id,
                PasswordLengthMax = passLengthMax, 
                PasswordLengthMin = passLengthMin, 
                PasswordAllowedUppercaseCount = upperCase,
                PasswordAllowedLowercaseCount = lowerCase,
                PasswordAllowedDigitCount = digit, 
                PasswordAllowedSpecialCharCount = specialChars, 
                MaxUserSessionSeconds = maxUserSessionSec, 
                LoginAccountLockAttempts = loginAttemptCount, 
                BruteForceBlockAttempts = bruteForceBlockAttempts, 
                BruteForceIdentificationSeconds = bruteForceIdSeconds, 
                BruteForceBlockSeconds = bruteForceBlockSec,
                LocationID = locationid 
            });
            unitOfWork.SaveChanges();
        }

    }
}
