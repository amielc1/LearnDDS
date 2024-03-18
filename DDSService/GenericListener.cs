using DDSService.Interface;
using OpenDDSharp.DDS;

namespace DDSService
{
     
    public class GenericListener<TData> : DataReaderListener
        where TData : class
    {
        private readonly DataReaderFactory<TData> _factory;
        public event EventHandler<TData> DataReceived = delegate { };
        private IGenericDataReader<TData> _genericDataReader;

        public GenericListener(DataReaderFactory<TData> factory)
        {
            _factory = factory;
        }

        protected override void OnDataAvailable(DataReader reader)
        {
            //if (_genericDataReader.GetType().Name == "MissionDataReaderAdapter")
            //{
            //    int t = 0; 
            //} 
           
            if (_genericDataReader == null) throw new InvalidOperationException("Invalid DataReader type.");

            var receivedData = new List<TData>();
            var receivedInfo = new List<SampleInfo>();
             _genericDataReader.Take(receivedData, receivedInfo, DataReceived);
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
