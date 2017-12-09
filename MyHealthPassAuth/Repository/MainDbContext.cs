using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.Repository
{
    public class MainDbContext : DbContext 
    {
        /// <summary>
        /// Represents collection of User entity
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Constructs a new context instance using the given options object.
        /// Multiple databases can be supported given the configurations passed 
        /// in the options object. 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {

        }
    }
}
