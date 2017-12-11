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
        private RequestData requestData; 

        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
            DataService.Instance.Initialize(this.testDbContext);
            ConfigService.Instance.Initialize(this.testDbContext);

            requestData = new RequestData
            {
                Request = "some.request",
                IpAddress = "192.68.1.1",
                UserAgent = "some.user.agent"
            }; 
        }

        [TestCleanup]
        public void TestTearDown()
        {
            TearDown(); 
        }

        [TestMethod]
        public void TestUserDoesNotExist()
        {
            TestUtils.AddAuthConfig(testDbContext, 1, 10, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1);
            ConfigService.Instance.LoadConfigurationList();
            TestUtils.AddUserToRepository(testDbContext,
                1,
                "test.user",
                "123456",
                0,
                1,
                new DateTime(2017, 12, 9, 0, 0, 0));

            RulesHandler rulesHandler = new RulesHandler(requestData);
            Message message = rulesHandler.HandleLogin("keshav.bahadoor", "123456");

            Assert.AreEqual(MessageResult.ERROR, message.Result);
            Assert.AreEqual("User does not exist", message.Text);
        }

        [TestMethod]
        public void TestCheckPasswordAgainstRulesError()
        {
            TestUtils.AddAuthConfig(testDbContext, 1, 10, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1);
            ConfigService.Instance.LoadConfigurationList();
            
            RulesHandler rulesHandler = new RulesHandler(requestData);
            Message message = rulesHandler.CheckPasswordAgainstRules("123456789awer", 1);

            Assert.AreEqual(MessageResult.ERROR, message.Result);
            Assert.AreEqual("Password length is too long", message.Text);
        }

        [TestMethod]
        public void TestSuccesfulLogin()
        {
            TestUtils.AddUserToRepository(testDbContext,
               1,
               "test.user",
               "123456",
               0,
               1,
               new DateTime(2017, 12, 9, 0, 0, 0));

            TestUtils.AddAuthConfig(testDbContext, 1, 10, 5, 1, 1, 1, 1, 10000, 3, 13, 600, 1200, 1);
            ConfigService.Instance.LoadConfigurationList();

            RulesHandler rulesHandler = new RulesHandler(requestData);
            Message message = rulesHandler.HandleLogin("test.user", "123456");

            Assert.AreEqual(MessageResult.SUCCESS, message.Result);
        }

        
    }
}
