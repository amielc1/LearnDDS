using HelloWorld;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorldPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Ace.Init();
      
            var dpf = ParticipantService.Instance.GetDomainParticipantFactory(
                new string[]
                {
                    "-DCPSConfigFile", "rtps_pub.ini", 
                    //"-DCPSDebugLevel", "10",
                    //"-ORBLogFile", "HelloWorldPublisher.log",
                    //"-ORBDebugLevel", "10"
                }
            );

            InitDDS(dpf, "Amiel", null);
            InitDDS(dpf, "Neria", null);//"config_5000");

            Console.WriteLine("Press a key to exit...");
            Console.Read();

            //participant.DeleteContainedEntities();
            //dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }

        private static void InitDDS(DomainParticipantFactory dpf, string participantName, string configName)
        {
            var participant = dpf.CreateParticipant(42);

            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            Console.WriteLine($"Creating participant {participantName} for domain 43, participant id {participant.GetHashCode()}");

            if (!string.IsNullOrEmpty(configName))
            {
                TransportRegistry.Instance.BindConfig(configName, participant);
            }


            MessageTypeSupport support = new();
            ReturnCode result = support.RegisterType(participant, support.GetTypeName());
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register type: " + result.ToString());
            }

            var test = support.GetTypeName();
            var topic = participant.CreateTopic("MessageTopic", support.GetTypeName());
            if (topic == null)
            {
                throw new Exception("Could not create the message topic");
            }

            var publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new Exception("Could not create the publisher");
            }

            //if (!string.IsNullOrEmpty(configName))
            //{
            //    TransportRegistry.Instance.BindConfig(configName, participant);
            //}

            var writer = publisher.CreateDataWriter(topic);
            if (writer == null)
            {
                throw new Exception("Could not create the data writer");
            }

            MessageDataWriter messageWriter = new(writer);

            Console.WriteLine($"Create MessageDataWriter on Topic {topic.Name}, Domain {topic.Participant.DomainId}");
             
            Console.WriteLine("Subscriber found, writting data....");
            Task.Run(() =>
            {
                var counter = 0;

                while (true)
                {
                    var message = $"{counter} {participantName}: Hello, I love you, won't you tell me your name?";
                    messageWriter.Write(new Message { Content = message });
                    Console.WriteLine(message);
                    counter++;
                    Thread.Sleep(500);
                }
            });
        }
    }
}
