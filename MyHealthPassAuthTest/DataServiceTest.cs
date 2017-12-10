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
    public class DataServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<MainDbContext> inMemoryOptions = new DbContextOptionsBuilder<MainDbContext>()
               .UseInMemoryDatabase(databaseName: "in_memory_db")
               .Options;

            DataService.Instance.Initialize(inMemoryOptions);
            DataService.Instance.DbContext.Database.EnsureDeleted(); 
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
    }
}
