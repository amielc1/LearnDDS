using MissionModule;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using System;
using System.Collections.Generic;

namespace MissionSubscriber
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

            Console.WriteLine($"Create Subscriber on participant {participant.DomainId} ");
            var subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new Exception("Could not create the subscriber");
            }


            Console.WriteLine($"Create DataReader on participant {participant.DomainId} ");
            var reader = subscriber.CreateDataReader(topic);
            if (reader == null)
            {
                throw new Exception("Could not create the  DataReader");
            }

            Console.WriteLine($"Wrap DataReader with MissionDataReader helper class");
            MissionDataReader missionDataReader = new(reader);

            while (true)
            {
                StatusMask mask = missionDataReader.StatusChanges;
                if ((mask & StatusKind.DataAvailableStatus) != 0)
                {
                    List<Mission> receivedData = new();
                    List<SampleInfo> receivedInfo = new();
                    result = missionDataReader.Take(receivedData, receivedInfo);

                    if (result == ReturnCode.Ok)
                    {
                        bool messageReceived = false;
                        for (int i = 0; i < receivedData.Count; i++)
                        {
                            if (receivedInfo[i].ValidData)
                            {
                                Console.WriteLine(receivedData[i].Name);
                                messageReceived = true;
                            }
                        }

                        //if (messageReceived)
                        //    break;
                    }
                }

                System.Threading.Thread.Sleep(100);
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
