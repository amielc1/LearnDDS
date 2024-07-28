using HelloWorld;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using System;

namespace HelloWorldSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Ace.Init();

            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps_sub.ini");
            
            DomainParticipant participant = dpf.CreateParticipant(42);
            if (participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            MessageTypeSupport support = new();
            ReturnCode result = support.RegisterType(participant, support.GetTypeName());
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register type: " + result.ToString());
            }

            Topic topic = participant.CreateTopic("MessageTopic", support.GetTypeName());
            if (topic == null)
            {
                throw new Exception("Could not create the message topic");
            }

            Subscriber subscriber = participant.CreateSubscriber();
            if (subscriber == null)
            {
                throw new Exception("Could not create the subscriber");
            }

            DataReader reader = subscriber.CreateDataReader(topic);
            if (reader == null)
            {
                throw new Exception("Could not create the message data reader");
            }
            MessageDataReader messageReader = new(reader);
            Console.WriteLine($"Create MessageDataReader on Topic {topic.Name}, Domain {topic.Participant.DomainId}");
            var listener = new GenericListener();
            listener.DataReceived += (sender, message) =>
            {
                Console.WriteLine($" <= {message}");
            };

            reader.SetListener(listener);
            var s = TransportRegistry.Instance.GlobalConfig;
            Console.WriteLine("Press a key to exit...");
            Console.Read();

            participant.DeleteContainedEntities();
            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();

            Ace.Fini();
        }
    }
}
