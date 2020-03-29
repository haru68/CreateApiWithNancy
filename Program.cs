using System;
using Nancy.Hosting.Self;
using Nancy.Configuration;

namespace CreateApi_NancyFk
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabasePopulator populator = new DatabasePopulator();
            populator.Populate(20);

            HostConfiguration hostConfiguration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true },
            };
            using (var host = new NancyHost(hostConfiguration, new Uri("http://localhost:1234")))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:1234");
                Console.ReadLine();
                host.Stop();
            }
        }
    }
}
