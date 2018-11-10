using Guytp.Logging;
using System;

namespace Guytp.WebApi.TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Core Test Service";
            new WebApi().Bootstrap();
            try
            {
                Logger.ApplicationInstance.Dispose();
            }
            catch
            {
                // Intentionally swallow
            }
        }
    }
}