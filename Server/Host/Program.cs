using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Services.DataService)))
            {

                host.Open();
                Console.WriteLine("Server is running");
                Console.ReadLine();


            }
        }
    }
}
