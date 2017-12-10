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
            AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "logincall?username=something",
                @"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                new DateTime(2017, 12, 9, 0, 0, 0));

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.AuthenticationLogs.Count());
                Assert.AreEqual("192.161.1.1", context.AuthenticationLogs.Single().IpAddress);
                Assert.AreEqual("logincall?username=something", context.AuthenticationLogs.Single().RequestData);
                Assert.AreEqual(@"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36", 
                    context.AuthenticationLogs.Single().UserAgent);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.AuthenticationLogs.Single().InsertDate);
            }
        }

        [TestMethod]
        public void TestLocationUpdate()
        {
            AddAuthenticationLog(testDbContext,
                1,
                "192.161.1.1",
                "logincall?username=something",
                @"Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                new DateTime(2017, 12, 9, 0, 0, 0));

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
            }
        }

        /// <summary>
        /// Adds a specified location to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        private void AddAuthenticationLog(MainDbContext context, int id, string ipAddress, string requestdata, string useragent, DateTime insertDate)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.AuthLogRepository.Add(new AuthenticationLog
            {
                AuthenticationLogID = id, 
                IpAddress = ipAddress, 
                RequestData = requestdata, 
                UserAgent = useragent,
                InsertDate = insertDate
            });
            unitOfWork.SaveChanges();
        }
    }
}
