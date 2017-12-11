using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
using MyHealthPassAuth.Services;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class DataServiceTest : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
            DataService.Instance.Initialize(this.testDbContext); 
        }

        [TestCleanup]
        public void TestTearDown()
        {
            TearDown();
        }

        [TestMethod]
        public void TestFindUserByUsername()
        {
            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                1, "userOne", "123456", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                2, "userTwo", "qwerty", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                3, "userThree", "12326", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            var user = DataService.Instance.FindUserByUsername("userTwo");
             
            Assert.AreEqual("userTwo", user.Username);
            Assert.AreEqual("qwerty", user.Password);
            Assert.AreEqual(1, user.LocationID);
            Assert.AreEqual(0, user.FailedLoginAttempts);
            Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), user.FailedLoginDateTime);
        }

        [TestMethod]
        public void TestFindUserByUsernameNull()
        {
            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                1, "userOne", "123456", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                2, "userTwo", "qwerty", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                3, "userThree", "12326", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            var user = DataService.Instance.FindUserByUsername("newUser");

            Assert.IsNull(user); 
        }

        [TestMethod]
        public void TestIncrementUserFailedLoginAttempts()
        {
            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                1, "userOne", "123456", 0, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            var user = DataService.Instance.FindUserByUsername("userOne");

            // Increment 
            DataService.Instance.IncrementUserFailedLoginAttempts(user);
            DataService.Instance.IncrementUserFailedLoginAttempts(user); 

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            { 
                Assert.AreEqual(2, context.Users.Single().FailedLoginAttempts); 
            }
        }

        [TestMethod]
        public void TestResetLoginAttempts()
        {
            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                1, "userOne", "123456", 2, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            var user = DataService.Instance.FindUserByUsername("userOne");

            DataService.Instance.ResetLoginAttempts(user);

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(0, context.Users.Single().FailedLoginAttempts);
            }
        }

        [TestMethod]
        public void TestLockUserAccount()
        {
            TestUtils.AddUserToRepository(DataService.Instance.DbContext,
                1, "userOne", "123456", 2, 1, new DateTime(2017, 12, 9, 0, 0, 0));

            var user = DataService.Instance.FindUserByUsername("userOne");

            DataService.Instance.LockUserAccount(user);

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual((int)AccountLockedEnum.LOCKED, context.Users.Single().AccountLocked);
            }
        }

        [TestMethod]
        public void TestAddAuthenticationLog()
        {
            DataService.Instance.AddAuthenticationLog(
                "192.161.1.1",
                "logincall?username=something",
                @"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                "success");

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthenticationLogs.Count());
                Assert.AreEqual("192.161.1.1", context.AuthenticationLogs.Single().IpAddress);
                Assert.AreEqual("logincall?username=something", context.AuthenticationLogs.Single().RequestData);
                Assert.AreEqual(@"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                    context.AuthenticationLogs.Single().UserAgent);
                Assert.AreEqual("success", context.AuthenticationLogs.Single().ResultMessage);
            }
        }

        [TestMethod]
        public void TestAddBlackListLog()
        {
            DataService.Instance.AddOrUpdateBlackListLog(
                "192.161.1.1",
                "user.agent" );

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.BlackListLogs.Count());
                Assert.AreEqual("192.161.1.1", context.BlackListLogs.Single().IpAddress);
                Assert.AreEqual("user.agent", context.BlackListLogs.Single().UserAgent); 
            }
        }

        [TestMethod]
        public void TestUpdateBlackListLog()
        {
            // Add record for 2 days in past 
            var unitOfWork = new UnitOfWork(testDbContext);
            unitOfWork.BlackListLogRepository.Add(new BlackListLog
            {
                BlackListID = 1,
                IpAddress = "192.161.1.1",
                UserAgent = "user.agent",
                FlagDate = DateTime.Now.AddDays(-2)
            });
            unitOfWork.SaveChanges();

            // Try adding via data service
            DataService.Instance.AddOrUpdateBlackListLog(
                "192.161.1.1",
                "user.agent");

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.BlackListLogs.Count());
                Assert.AreEqual("192.161.1.1", context.BlackListLogs.Single().IpAddress);
                Assert.AreEqual("user.agent", context.BlackListLogs.Single().UserAgent);

                // Ensure its today
                Assert.AreEqual(DateTime.Now.Day, context.BlackListLogs.Single().FlagDate.Value.Day);
                Assert.AreEqual(DateTime.Now.Month, context.BlackListLogs.Single().FlagDate.Value.Month);
                Assert.AreEqual(DateTime.Now.Year, context.BlackListLogs.Single().FlagDate.Value.Year);
            }
        }

        [TestMethod]
        public void TestGetAuthLogsCorrect()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-2),
                "Error fail");

            int count = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                600);

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestGetAuthLogsIncorrectData()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-2),
                "Error fail");

            int count = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                "192.161.1.1",
                "other.request.data",
                "my.user.agent",
                600);

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestGetAuthLogsTooLongAgo()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-20),
                "Error fail");

            int count = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                600);

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestGetAuthLogsSuccess()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-2),
                "success");

            int count = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                600);

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestGetAuthLogsMultiple()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-2),
                "Error fail");

            TestUtils.AddAuthenticationLog(testDbContext,
                2,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-3),
                "Error fail");

            TestUtils.AddAuthenticationLog(testDbContext,
                3,
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                DateTime.Now.AddMinutes(-4),
                "Error fail");

            int count = DataService.Instance.GetAmountOfFailedAttemptsForLastSeconds(
                "192.161.1.1",
                "my.request.data",
                "my.user.agent",
                600);

            Assert.AreEqual(3, count);
        }
    }
}
