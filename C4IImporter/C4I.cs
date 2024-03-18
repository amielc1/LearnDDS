using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;
using OpenDDSharp.DDS;

namespace CombatSystemDemo.Devices
{
    public class C4I
    {
        private readonly string MissionTopic = "MissionTopic";
        private readonly IDataReaderCreator _readerLoc;
        private readonly IDataReaderCreator _readerMis;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly ISubscriber _subscriber;
        private readonly ISubscriber _subscriberMis;

        public C4I()
        {

            IGenericDataReader<Location> ReaderFactory(DataReader reader) => new LocationDataReaderAdapter(reader);
            _readerLoc = new GenericReaderCreator<LocationTypeSupportAdapter, Location>(ReaderFactory);


            IGenericDataReader<Mission> ReaderFactoryMis(DataReader reader) => new MissionDataReaderAdapter(reader);
            _readerMis = new GenericReaderCreator<MissionTypeSupportAdapter, Mission>(ReaderFactoryMis);


            _config = new DdsConfiguration
            {
                Topic = "LocationTopic"
            };
            _ddsService = new OpenDdsService(_config);
            _subscriber = new DdsSubscriber(_ddsService, _readerLoc);
            _subscriberMis = new DdsSubscriber(_ddsService, _readerMis);
        }

        public async Task Import()
        {
            await _subscriber.Subscribe(_config.Topic, OnMessageArrived);
            Console.WriteLine($"C4I Subscribe to {_config.Topic}");
        }

        public async Task ImportMis()
        {
            await _subscriberMis.Subscribe(MissionTopic, OnMessageArrivedMis);
            Console.WriteLine($"C4I Subscribe to {MissionTopic}");
        }

        private void OnMessageArrivedMis(object? sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} C4I RCV Mission  * {((Mission)e).Name} from topic {_config.Topic}");

        }

        private void OnMessageArrived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} C4I RCV Location * {((Location)e).Key}");
        }
    }
}
