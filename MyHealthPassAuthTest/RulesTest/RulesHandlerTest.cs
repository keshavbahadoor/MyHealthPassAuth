using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
using MyHealthPassAuth.RuleEngine.Rules;
using MyHealthPassAuth.System;
using MyHealthPassAuth.Services;
using MyHealthPassAuth.RuleEngine;

namespace MyHealthPassAuthTest.RulesTest
{
    [TestClass]
    public class RulesHandlerTest : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
            DataService.Instance.Initialize(this.testDbContext);
            ConfigService.Instance.Initialize(this.testDbContext);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            TearDown(); 
        }

        [TestMethod]
        public void TestUserDoesNotExist()
        {
            TestUtils.AddUserToRepository(testDbContext,
                1,
                "test.user",
                "123456",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));

            RulesHandler rulesHandler = new RulesHandler();
            Message message = rulesHandler.HandleLogin("keshav.bahadoor", "123456");

            Assert.AreEqual(MessageResult.ERROR, message.Result);
            Assert.AreEqual("User does not exist", message.Text);
        }

        
    }
}
