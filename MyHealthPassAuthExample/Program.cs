using Microsoft.EntityFrameworkCore;
using MyHealthPassAuth;
using MyHealthPassAuth.Repository;
using MyHealthPassAuth.System;
using System;
using System.Linq;

namespace MyHealthPassAuthExample
{
    class Program
    {
        static string mysqlConnString = @"Server=localhost;database=my_health_pass_auth;uid=root;pwd=pass;";

        static void Main(string[] args)
        {
            Console.WriteLine("This is an exmaple of using MyHealthPassAuth library\n\n");

            // Set up options object 
            var mysqlOptions = new DbContextOptionsBuilder<MainDbContext>()
                .UseMySql(mysqlConnString)
                .Options; 

            // Create AuthLib object 
            AuthLib app = new AuthLib(mysqlOptions, new ConsoleLog());

            // Failed login attempt 
            Console.WriteLine(); 
            Message message = app.Login("keshav", "p2323", "useragent", "login", "requestdata");
            Console.WriteLine("Returned Message: " + message.Text);

            // Successful Login 
            Console.WriteLine();
            message = app.Login("keshav", "password", "useragent", "login", "requestdata");
            Console.WriteLine("Returned Message: " + message.Text);

            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
