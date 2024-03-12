using MissionModule;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using System;

namespace MissionPublisher
{
    internal class Program
    {
        static string _topic = "MissionTopic";
        static void Main(string[] args)
        {
            Console.WriteLine("Init DDS Sharp");
            Ace.Init();

            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
            Console.WriteLine("Create DomainParticipant (42)");
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            // Include your program here

            MissionTypeSupport missionTypeSupport = new();
            var missionTypeName = missionTypeSupport.GetTypeName();
            ReturnCode result = missionTypeSupport.RegisterType(participant, missionTypeName);
            Console.WriteLine($"Register participant {participant.DomainId}, with MissionType {missionTypeName} ");
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register type: " + result.ToString());
            }

            Console.WriteLine($"Create Topic {_topic} with participant {participant.DomainId} and MissionType {missionTypeName} ");
            var topic = participant.CreateTopic(_topic, missionTypeName);
            if (topic == null)
            {
                throw new Exception("Could not create the message topic");
            }

            Console.WriteLine($"Create Publisher on participant {participant.DomainId} ");
            var publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new Exception("Could not create the publisher");
            }

            Console.WriteLine($"Create DataWriter on participant {participant.DomainId} ");
            var writer = publisher.CreateDataWriter(topic);
            if (writer == null)
            {
                throw new Exception("Could not create the data writer");
            }

            Console.WriteLine($"Wrap DataWriter with MissionDataWriter helper class");
            MissionDataWriter messageWriter = new(writer);


            Console.WriteLine("Waiting for a subscriber...");
            PublicationMatchedStatus status = new();
            do
            {
                _ = messageWriter.GetPublicationMatchedStatus(ref status);
                System.Threading.Thread.Sleep(500);
            }
            while (status.CurrentCount < 1);

            Console.WriteLine("Subscriber found, writing data.");
            while (true)
            {
                messageWriter.Write(new Mission()
                {
                    Key = 1,
                    Name = "Mission 1",
                    Description = "General Mission",
                    Status = "Created"
                });
                Console.Write(".");
            }



            Console.WriteLine("Press a key to exit...");
            Console.Read();
             

            participant.DeleteContainedEntities();
            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }
    }
}
