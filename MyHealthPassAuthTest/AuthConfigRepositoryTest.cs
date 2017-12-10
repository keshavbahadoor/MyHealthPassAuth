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
            TestUtils.AddAuthConfig(this.testDbContext, 1, 15, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1); 

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
            TestUtils.AddAuthConfig(this.testDbContext, 1, 15, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1);

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



    }
}
