using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    [TestClass]
    public class TestLocationRepository : AbstractRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
        } 

        [TestMethod]
        public void TestLocationInsert()
        {
            AddLocation(testDbContext, 1, "Trinidad", "Chaguanas");

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.Locations.Count());
                Assert.AreEqual("Trinidad", context.Locations.Single().Country);
                Assert.AreEqual("Chaguanas", context.Locations.Single().Region);
            }
        }

        [TestMethod]
        public void TestLocationUpdate()
        {
            AddLocation(testDbContext, 1, "Trinidad", "Chaguanas");

            var unitOfWork = new UnitOfWork(testDbContext);
            var location = unitOfWork.LocationRepository.Entities
                    .First(l => l.LocationID == 1);

            location.Country = "United Kingdom";
            location.Region = "London";
            unitOfWork.Commit();

            // Separate instance of context to verify data insert 
            using (var context = new MainDbContext(inMemoryOptions))
            {
                Assert.AreEqual(1, context.Locations.Count());
                Assert.AreEqual("United Kingdom", context.Locations.Single().Country);
                Assert.AreEqual("London", context.Locations.Single().Region);
            }
        }

        /// <summary>
        /// Adds a specified location to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        private void AddLocation(MainDbContext context, int id, string country, string region)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.LocationRepository.Add(new Location
            {
                LocationID = id,
                Country = country,
                Region = region
            });
            unitOfWork.Commit();
        }
    }
     
}
