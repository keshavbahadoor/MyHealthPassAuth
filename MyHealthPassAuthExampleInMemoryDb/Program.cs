using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.System;
using System;
using System.Linq;

namespace MyHealthPassAuthExampleInMemoryDb
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an exmaple of using MyHealthPassAuth library with EF Core In Memory DB\n\n");

            // Set up options object 
            var inMemoryOptions = new DbContextOptionsBuilder<MainDbContext>()
                .UseInMemoryDatabase(databaseName: "in_memory_db")
                .EnableSensitiveDataLogging(true)
                .Options;

            // Populate Example Data 
            PopulateData.Populate(new MainDbContext(inMemoryOptions));

            // Create AuthLib object 
            AuthLib app = new AuthLib(inMemoryOptions, new ConsoleLog());            

            // Failed login attempt 
            Console.WriteLine();
            Message message = app.Login("keshav", "p2323", "useragent", "login", "requestdata");
            Console.WriteLine("Returned Message: " + message.Text);

            // Successful Login 
            Console.WriteLine();
            message = app.Login("keshav", "password", "useragent", "login", "requestdata");
            Console.WriteLine("Returned Message: " + message.Text);

            // locked account 
            for (int i=0; i< 5; i++ )
            {
                Console.WriteLine();
                message = app.Login("keshav", "p2323", "useragent", "login", "requestdata");
                Console.WriteLine("Returned Message: " + message.Text);
            }

            // Brute Blocked attempt
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine();
                message = app.Login("hacker", "12345", "useragent2", "login", "requestdata2");
                Console.WriteLine("Returned Message: " + message.Text);
            }
             

            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
