using DDSService.Interface;
using OpenDDSharp.DDS;

namespace DDSService
{
    public class GenericReaderCreator<TTypeSupport, TData> : IDataReaderCreator
        where TTypeSupport : ITypeSupport, new()
        where TData : class
    {
        public event EventHandler<object> DataReceived;
        private Subscriber _subscriber;
        private DomainParticipant _participant;
        private CancellationTokenSource _cancellationTokenSource = new();
        private GenericListener<TData> _listener;

        public GenericReaderCreator(DataReaderFactory<TData> factory)
        {
            _listener = new GenericListener<TData>(factory);
            _listener.DataReceived += (s, e) => DataReceived?.Invoke(s, e);
        }

        public DataReader Subscribe(DomainParticipant participant, string topic)
        {
            _participant = participant;
            RegisterType(topic);
            CreateSubscriber();
            return CreateAndWrapDataReader(topic);
        }

        private void RegisterType(string topic)
        {
            TTypeSupport typeSupport = new TTypeSupport();
            var typeName = typeSupport.GetTypeName();
            var result = typeSupport.RegisterType(_participant, typeName);
            CheckResult(result, $"Could not register type: {typeName}");

            var topicInstance = _participant.CreateTopic(topic, typeName);
            CheckNotNull(topicInstance, "Could not create the message topic");
        }

        private void CreateSubscriber()
        {
            _subscriber = _participant.CreateSubscriber();
            CheckNotNull(_subscriber, "Could not create the subscriber");
        }

        private DataReader CreateAndWrapDataReader(string topic)
        {
            var dataReader = _subscriber.CreateDataReader(_participant.LookupTopicDescription(topic), null, _listener,
                StatusMask.AllStatusMask);
            CheckNotNull(dataReader, "Could not create the DataReader");
            return dataReader;
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
