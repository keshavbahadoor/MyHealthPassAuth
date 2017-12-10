using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;
using MyHealthPassAuth.Services;
using System.Collections.Generic;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class ConfigServiceTest
    { 
        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<MainDbContext> inMemoryOptions = new DbContextOptionsBuilder<MainDbContext>()
               .UseInMemoryDatabase(databaseName: "in_memory_db_config")
               .EnableSensitiveDataLogging(true) 
               .Options;

            ConfigService.Instance.Initialize(inMemoryOptions);
            ConfigService.Instance.DbContext.Database.EnsureDeleted();
        }

        [TestCleanup]
        public void TearDown()
        { 
            ConfigService.Instance.DbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void TestLocalConfigListContent()
        {
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 1, 15, 5, 1, 1, 1, 1, 123, 3, 13, 600, 1200, 1);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 2, 20, 15, 1, 1, 1, 1, 1000, 3, 14, 1600, 1200, 2);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 3, 11, 5, 2, 1, 1, 1, 100, 3, 13, 600, 1200, 3);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 4, 22, 5, 1, 2, 1, 1, 10000, 3, 15, 600, 1200, 4);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 5, 12, 5, 1, 1, 2, 1, 10000, 3, 13, 100, 1200, 5);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 6, 15, 5, 1, 2, 1, 1, 10000, 3, 13, 1600, 1200, 6);

            ConfigService.Instance.LoadConfigurationList();

            CollectionAssert.AreEqual(ConfigService.Instance.Configs,
                ConfigService.Instance.DbContext.AuthorizationConfig.ToList());
        }

        [TestMethod]
        public void TestGetConfigsForLocationID()
        {
            LoadLocationsAndConfigs();

            AuthorizationConfig config = ConfigService.Instance.GetConfigForLocationID(2);
             
            Assert.AreEqual(10, config.PasswordLengthMax);
            Assert.AreEqual(2, config.PasswordLengthMin);
            Assert.AreEqual(2, config.PasswordAllowedUppercaseCount);
            Assert.AreEqual(4, config.PasswordAllowedLowercaseCount);
            Assert.AreEqual(2, config.PasswordAllowedDigitCount);
            Assert.AreEqual(1, config.PasswordAllowedSpecialCharCount);
            Assert.AreEqual(10000, config.MaxUserSessionSeconds);
            Assert.AreEqual(2, config.LoginAccountLockAttempts);
            Assert.AreEqual(10, config.BruteForceBlockAttempts);
            Assert.AreEqual(100, config.BruteForceIdentificationSeconds);
            Assert.AreEqual(1200, config.BruteForceBlockSeconds);
            Assert.AreEqual(2, config.LocationID);
        }

        [TestMethod]
        public void TestGetConfigsForLocationIDNull()
        {
            LoadLocationsAndConfigs();

            AuthorizationConfig config = ConfigService.Instance.GetConfigForLocationID(4);

            Assert.IsNull(config);
        }

        private void LoadLocationsAndConfigs()
        {
            // Add some locations 
            TestUtils.AddLocation(ConfigService.Instance.DbContext, 1, "Trinidad", "Chaguanas");
            TestUtils.AddLocation(ConfigService.Instance.DbContext, 2, "United Kingdom", "London");
            TestUtils.AddLocation(ConfigService.Instance.DbContext, 3, "Australia", "Melbourne");

            // Add some auth configs 
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 1, 15, 5, 1, 1, 1, 1, 123, 3, 13, 600, 1200, 1);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 2, 10, 2, 2, 4, 2, 1, 10000, 2, 10, 100, 1200, 2);
            TestUtils.AddAuthConfig(ConfigService.Instance.DbContext, 3, 11, 5, 2, 1, 1, 1, 100, 3, 13, 500, 1200, 3);

            ConfigService.Instance.LoadConfigurationList();
        }
    }
}
