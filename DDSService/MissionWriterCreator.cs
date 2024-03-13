using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService
{
    public class MissionWriterCreator : IDataWriterCreator
    {
        private DomainParticipant _participant;
        private Publisher? _publisher;
        private Topic _topicInstance;
        MissionDataWriter _dataWriter;
        public DataWriter CreateWriter(DomainParticipant participant, string topic)
        {
            try
            {
                if (_dataWriter != null)
                    return _dataWriter;

                _participant = participant;
                RegisterType(participant, topic);
                _publisher = CreatePublisher(_participant);
                _dataWriter = CreateAndWrapDataWriter(_participant);
                //WaitingToSubscriber();
                return _dataWriter;
            }
            catch (Exception ex)
            {
                LogError($"Error creating data reader: {ex.Message}");
                throw;
            }
        }

        private MissionDataWriter CreateAndWrapDataWriter(DomainParticipant participant)
        {
            Console.WriteLine($"Create DataWriter on participant {participant.DomainId} ");
            var writer = _publisher.CreateDataWriter(_topicInstance);
            if (writer == null)
            {
                throw new Exception("Could not create the data writer");
            }

            Console.WriteLine($"Wrap DataWriter with MissionDataWriter helper class");
            MissionDataWriter messageWriter = new(writer);
            return messageWriter;

        }

        private static Publisher? CreatePublisher(DomainParticipant participant)
        {
            Console.WriteLine($"Create Publisher on participant {participant.DomainId} ");
            var publisher = participant.CreatePublisher();
            if (publisher == null)
            {
                throw new Exception("Could not create the publisher");
            }

            return publisher;
        }

        private void RegisterType(DomainParticipant participant, string topic)
        {
            var missionTypeSupport = new MissionTypeSupport();
            var missionTypeName = missionTypeSupport.GetTypeName();
            var result = missionTypeSupport.RegisterType(participant, missionTypeName);

            Log($"Register participant {participant.DomainId}, with MissionType {missionTypeName}");
            if (result != ReturnCode.Ok)
            {
                throw new Exception($"Could not register type: {result}");
            }

            _topicInstance = participant.CreateTopic(topic, missionTypeName);
            Log($"Create Topic {topic} with participant {participant.DomainId} and MissionType {missionTypeName}");
            if (_topicInstance == null)
            {
                throw new Exception("Could not create the message topic");
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void LogError(string message)
        {
            Console.Error.WriteLine(message);
        }

        public Task Publish(object data)
        {
            try
            { 
                _dataWriter.Write((Mission)data);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private void WaitingToSubscriber()
        {
            Console.WriteLine("Waiting for a subscriber...");
            PublicationMatchedStatus status = new();
            do
            {
                _ = _dataWriter.GetPublicationMatchedStatus(ref status);
            } while (status.CurrentCount < 1);

            Console.WriteLine("Subscriber found, writing data.");
        }
    }
}
