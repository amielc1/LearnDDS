using MissionModule;
using System;

namespace MissionPublisher;

internal class Program
{
    private static int counter = 0;
    static void Main(string[] args)
    {

        var exporter = new DDSExporter();

        while (true)
        {
            var msg = new Mission()
            {
                Key = counter++,
                Name = $"Mission {counter}",
                Description = "General Mission",
                Status = "Created"
            };
            exporter.Export(msg);
            Console.WriteLine($" = > Publish {msg.Name}");
        }
    }
}