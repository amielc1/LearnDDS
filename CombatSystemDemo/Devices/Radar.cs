using CombatSystem.Models;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.Configuration;

namespace CombatSystem.Devices
{
    public class Radar
    {
        private readonly DdsConfiguration _ddsConfiguration; 
        private DomainParticipant _participant;
        private DomainParticipantFactory _dpf;

        public Radar()
        {
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
            Console.WriteLine("Create DomainParticipant (42)");
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            // Include your program here

            
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
        }

        public void SendLocation(Location location)
        {
              
          
        }
    }
}
