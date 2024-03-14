using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService
{ 

    public class MissionReaderCreator : IDataReaderCreator
    {
        public event EventHandler<object> DataReceived = delegate { };
        private Subscriber _subscriber;
        private DomainParticipant _participant;
        private CancellationTokenSource _cancellationTokenSource = new();
        private MissionListener listener = new();

        public MissionReaderCreator()
        {
            listener.DataReceived += (s, e) => DataReceived(s, e);
        }

        public DataReader Subscribe(DomainParticipant participant, string topic)
        {
            try
            {
                _participant = participant;
                RegisterType(participant, topic);
                CreateSubscriber(participant);
                return CreateAndWrapDataReader(topic);
            }
            catch (Exception ex)
            {
                LogError($"Error creating data reader: {ex.Message}");
                throw;
            }
        }

        private void RegisterType(DomainParticipant participant, string topic)
        {
            var missionTypeSupport = new MissionTypeSupport();
            var missionTypeName = missionTypeSupport.GetTypeName();
            var result = missionTypeSupport.RegisterType(participant, missionTypeName);

            Log($"Register participant {participant.DomainId}, with MissionType {missionTypeName}");
            CheckResult(result, $"Could not register type: {result}");

            var topicInstance = participant.CreateTopic(topic, missionTypeName);
            Log($"Create Topic {topic} with participant {participant.DomainId} and MissionType {missionTypeName}");
            CheckNotNull(topicInstance, "Could not create the message topic");

        }

        private void CreateSubscriber(DomainParticipant participant)
        {
            _subscriber = participant.CreateSubscriber();
            Log($"Create Subscriber on participant {participant.DomainId}");
            CheckNotNull(_subscriber, "Could not create the subscriber");
        }

        private MissionDataReader CreateAndWrapDataReader(string topic)
        {
            var dataReader = _subscriber.CreateDataReader(
                _participant.LookupTopicDescription(topic),
                null, // Use default QoS policies
                listener, // Attach your listener here
                StatusMask.AllStatusMask); // Specify the statuses you're interested in

            Console.WriteLine($"Create DataReader on participant {_participant.DomainId}");
            CheckNotNull(dataReader, "Could not create the DataReader");
            var missionDataReader = new MissionDataReader(dataReader);
            Console.WriteLine("Wrap DataReader with MissionDataReader helper class");
            return missionDataReader;
        }

        public void UnSubscribe()
        {
            _cancellationTokenSource.Cancel();
            _participant.DeleteSubscriber(_subscriber);
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void LogError(string message)
        {
            Console.Error.WriteLine(message);
        }
        private void CheckResult(ReturnCode result, string errorMessage)
        {
            if (result != ReturnCode.Ok) throw new Exception(errorMessage);
        }

        private void CheckNotNull(object obj, string errorMessage)
        {
            if (obj == null) throw new Exception(errorMessage);
        }
    }
}
