using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
namespace MyHealthPassAuthTest
{
    [TestClass]
    public class BlackListLogRepositoryTest : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
        }

        [TestMethod]
        public void TestBlacklistInsert()
        {
            var unitOfWork = new UnitOfWork(this.testDbContext);
            unitOfWork.BlackListLogRepository.Add(new BlackListLog
            {
                BlackListID = 1,
                IpAddress = "150.123.111.22",
                UserAgent = "agent",
                FlagDate = new DateTime(2017, 12, 9, 0, 0, 0)
            });
            unitOfWork.SaveChanges();

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.BlackListLogs.Count());
                Assert.AreEqual("150.123.111.22", context.BlackListLogs.Single().IpAddress);
                Assert.AreEqual("agent", context.BlackListLogs.Single().UserAgent);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.BlackListLogs.Single().FlagDate);
            }
        }

        [TestMethod]
        public void TestBlacklistUpdate()
        {
            var unitOfWork = new UnitOfWork(this.testDbContext);
            unitOfWork.BlackListLogRepository.Add(new BlackListLog
            {
                BlackListID = 1,
                IpAddress = "150.123.111.22",
                UserAgent = "agent",
                FlagDate = new DateTime(2017, 12, 9, 0, 0, 0)
            });
            unitOfWork.SaveChanges();

            var log = unitOfWork.BlackListLogRepository.Entities
                    .First(u => u.BlackListID == 1);

            log.IpAddress = "190.111.222.12";
            log.UserAgent = "new agent";
            unitOfWork.SaveChanges();

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.BlackListLogs.Count());
                Assert.AreEqual("190.111.222.12", context.BlackListLogs.Single().IpAddress);
                Assert.AreEqual("new agent", context.BlackListLogs.Single().UserAgent);
                Assert.AreEqual(new DateTime(2017, 12, 9, 0, 0, 0), context.BlackListLogs.Single().FlagDate);
            }
        }

    }
}
