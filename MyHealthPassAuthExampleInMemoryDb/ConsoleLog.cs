using MyHealthPassAuth.System.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuthExampleInMemoryDb
{
    public class ConsoleLog : ILog
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(" >> " + message);
        }
    }
}
