using OpenDDSharp;
using System;
using System.Net.Security;
using MissionModule;
using MissionSubscriber.Interface;
using System.Reflection;

namespace MissionSubscriber
{
    internal class Program
    {
        static string _topic = "MissionTopic";
        static void Main(string[] args)
        {
            Console.WriteLine("Init DDS Sharp");

            IImporter importer = new DDSImporter();
            importer.Start();
            importer.DataReceived += Importer_DataReceived;

            Console.WriteLine("Press a key to exit...");
            Console.Read();
            Ace.Fini();
        }

        private static void Importer_DataReceived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}  {((Mission)e).Name}");
        }


    }
}
