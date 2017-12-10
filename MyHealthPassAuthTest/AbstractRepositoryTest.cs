using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Linq;

namespace MyHealthPassAuthTest
{
    /// <summary>
    /// Used for testing all repository instances 
    /// </summary>
    public class AbstractRepositoryTest
    {
        /// <summary>
        /// in memory database context option object 
        /// </summary>
        protected DbContextOptions<MainDbContext> inMemoryOptions;

        /// <summary>
        /// Context reference 
        /// </summary>
        protected MainDbContext testDbContext;


        /// <summary>
        /// Create in memory database and set context to this 
        /// </summary>
        protected void Initialize()
        {
            this.inMemoryOptions = new DbContextOptionsBuilder<MainDbContext>()
                .UseInMemoryDatabase(databaseName: "in_memory_db")
                .Options;

            testDbContext = new MainDbContext(inMemoryOptions);
            testDbContext.Database.EnsureDeleted();
        }

        /// <summary>
        /// Deletes database 
        /// </summary>
        protected void TearDown()
        {
            testDbContext.Database.EnsureDeleted();
        }

    }
}
