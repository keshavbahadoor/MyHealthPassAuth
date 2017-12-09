using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities; 
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        private DbContextOptions<MainDbContext> inMemoryOptions; 

        [TestInitialize]
        public void Initialize()
        {
            this.inMemoryOptions = new DbContextOptionsBuilder<MainDbContext>()
                .UseInMemoryDatabase(databaseName: "in_memory_db")
                .Options;
        }

        [TestMethod]
        public void TestUserInsert()
        { 
            // Run test against one instance of the context 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                var unitOfWork = new UnitOfWork(context);
                unitOfWork.UserRepository.Add(new User
                {
                    UserID = 1,
                    Username = "test.user",
                    FailedLoginAttempts = 0,
                    FailedLoginDateTime = new DateTime(2017, 12, 9, 0, 0, 0),
                    LocationID = 1,
                    Password = "123456"
                });

                unitOfWork.Commit(); 
            }

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
         
    }
}
