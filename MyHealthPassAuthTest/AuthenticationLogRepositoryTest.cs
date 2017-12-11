using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class AuthenticationLogRepositoryTest : AbstractRepositoryTest
    { 
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize(); 
        }
         

        [TestMethod]
        public void TestAuthLogInsert()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "logincall?username=something",
                @"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                new DateTime(2017, 12, 9, 0, 0, 0), 
                "success");

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthenticationLogs.Count());
                Assert.AreEqual("192.161.1.1", context.AuthenticationLogs.Single().IpAddress);
                Assert.AreEqual("logincall?username=something", context.AuthenticationLogs.Single().RequestData);
                Assert.AreEqual(@"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36", 
                    context.AuthenticationLogs.Single().UserAgent);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.AuthenticationLogs.Single().InsertDate);
                Assert.AreEqual("success", context.AuthenticationLogs.Single().ResultMessage);
            }
        }

        [TestMethod]
        public void TestLocationUpdate()
        {
            TestUtils.AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "logincall?username=something",
                @"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                new DateTime(2017, 12, 9, 0, 0, 0),
                "success");

            var unitOfWork = new UnitOfWork(testDbContext);
            var authlog = unitOfWork.AuthLogRepository.Entities
                    .First(a => a.AuthenticationLogID == 1);

            authlog.UserAgent = "updated-agent"; 
            unitOfWork.SaveChanges();

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthenticationLogs.Count());
                Assert.AreEqual("192.161.1.1", context.AuthenticationLogs.Single().IpAddress);
                Assert.AreEqual("logincall?username=something", context.AuthenticationLogs.Single().RequestData);
                Assert.AreEqual(@"updated-agent", context.AuthenticationLogs.Single().UserAgent);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.AuthenticationLogs.Single().InsertDate);
                Assert.AreEqual("success", context.AuthenticationLogs.Single().ResultMessage);
            }
        }

        
    }
}
