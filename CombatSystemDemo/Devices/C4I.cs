using System;
using System.Threading.Tasks;
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
        static int counter = 0;
        private readonly IDataReaderCreator _reader;
        private readonly IDataWriterCreator _writer;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly ISubscriber _subscriber;
        private readonly IPublisher _publisher;


        private const string MissionTopic = "MissionTopic";

        public C4I()
        {
            IGenericDataWriter<Mission> WriterFactory(DataWriter writer) => new MissionDataWriterAdapter(writer);
            _writer = new GenericWriterCreator<MissionTypeSupportAdapter, Mission>(WriterFactory);


            IGenericDataReader<Location> ReaderFactory(DataReader reader) => new LocationDataReaderAdapter(reader);
            _reader = new GenericReaderCreator<LocationTypeSupportAdapter, Location>(ReaderFactory);
 

            _config = new DdsConfiguration
            {
                Topic = "LocationTopic"
            };
            _ddsService = new OpenDdsService(_config);
            _subscriber = new DdsSubscriber(_ddsService, _reader);
            _publisher = new DdsPublisher(_ddsService, _writer);
        }

        public async Task Import()
        {
            await _subscriber.Subscribe(_config.Topic, OnMessageArrived);
            Console.WriteLine($"C4I Subscribe to {_config.Topic}");
        }

        public async Task ExportMission()
        { 
                 var msg = new Mission()
                {
                    Key = counter,
                    Name = $"{counter} mission",
                    Description = "Fire Command mission ",
                    Status = "to fire"
                };
                await _publisher.Publish(MissionTopic, msg);
                await Task.Delay(100);
                Console.WriteLine($"C4I SEND Mission {msg.Name} to topic {MissionTopic}");     }

        private async void OnMessageArrived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} C4I RCV Location {((Location)e).Key}");
            if ((counter++ % 10) == 0 )
            {
                await ExportMission(); 
            }
        }
    }
}
