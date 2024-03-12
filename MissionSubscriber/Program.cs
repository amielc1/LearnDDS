using OpenDDSharp;
using System;
using System.Net.Security;
using MissionModule;
using MissionSubscriber.Interface;

namespace MissionSubscriber
{
    internal class Program
    {
        static string _topic = "MissionTopic";
        static void Main(string[] args)
        {
            Console.WriteLine("Init DDS Sharp");

           

          

            Console.WriteLine("Press a key to exit...");
            Console.Read();
            Ace.Fini();
        }

        private static void Service_DataReceived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}  {((Mission)e).Name}");
        }
    }
}
