using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities; 
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class UserRepositoryTest : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
        }

        [TestMethod]
        public void TestUserInsert()
        {                          
            AddUserToRepository(testDbContext,
                1,
                "test.user",
                "123456",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));           

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            { 
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreEqual("test.user", context.Users.Single().Username);
                Assert.AreEqual("123456", context.Users.Single().Password);
                Assert.AreEqual(1, context.Users.Single().LocationID);
                Assert.AreEqual(0, context.Users.Single().FailedLoginAttempts);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.Users.Single().FailedLoginDateTime);
            }
        }

        [TestMethod]
        public void TestUserUpdate()
        {
            AddUserToRepository(testDbContext,
                1,
                "test.user",
                "123456",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));

            var unitOfWork = new UnitOfWork(testDbContext);
            var user = unitOfWork.UserRepository.Entities
                    .First(u => u.UserID == 1);

            user.Username = "updated.username";
            user.Password = "Password123#";
            unitOfWork.Commit();              

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreEqual("updated.username", context.Users.Single().Username);
                Assert.AreEqual("Password123#", context.Users.Single().Password);
                Assert.AreEqual(1, context.Users.Single().LocationID);
                Assert.AreEqual(0, context.Users.Single().FailedLoginAttempts);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.Users.Single().FailedLoginDateTime);
            }
        }

        /// <summary>
        /// Adds a specified user to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="failedAttempts"></param>
        /// <param name="locationid"></param>
        /// <param name="failedLoginDate"></param>
        private void AddUserToRepository(MainDbContext context, int id, string username, 
            string password, int failedAttempts, int locationid, DateTime failedLoginDate)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.UserRepository.Add(new User
            {
                UserID = id,
                Username = username,
                FailedLoginAttempts = failedAttempts,
                FailedLoginDateTime = failedLoginDate,
                LocationID = locationid,
                Password = password
            });
            unitOfWork.Commit();
        }
         
    }
}
