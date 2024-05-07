using DDSService.Interface;
using OpenDDSharp.DDS;

namespace DDSService
{

    public class GenericListener : DataReaderListener
    {
        private readonly DataReaderFactory _factory;
        public event EventHandler<object> DataReceived = delegate { };
        private IGenericDataReader _genericDataReader;

        public GenericListener(DataReaderFactory factory)
        {
            _factory = factory;
        }

        protected override void OnDataAvailable(DataReader reader)
        {
            if (_genericDataReader == null) throw new InvalidOperationException("Invalid DataReader type.");
            _genericDataReader.Take(DataReceived);
        }

        protected override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            Console.WriteLine($"OnRequestedDeadlineMissed {status}");
        }

        protected override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            Console.WriteLine($"OnRequestedIncompatibleQos {status}");
        }

        protected override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            Console.WriteLine($"OnSampleRejected {status}");
        }

        protected override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            Console.WriteLine($"OnLivelinessChanged {status}");
        }

        protected override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            Console.WriteLine($"OnSubscriptionMatched {status}");
            _genericDataReader = _factory(reader);
        }

        protected override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            Console.WriteLine($"OnSampleLost {status}");
        }
    }
}
