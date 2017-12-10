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
    }
}
