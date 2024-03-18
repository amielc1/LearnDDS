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
        private readonly IDataReaderCreator _reader;
        private readonly IDataWriterCreator _writer;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly ISubscriber _subscriber;
        private readonly IPublisher _publisher;


        private readonly string MissionTopic = "MissionTopic";
        public C4I()
        {

            IGenericDataReader<Location> Factory(DataReader reader) => new LocationDataReaderAdapter(reader);
            _reader = new GenericReaderCreator<LocationTypeSupportAdapter, Location>(Factory);

            DataWriterFactory.Register(writer => new MissionDataWriterAdapter(writer));
            _writer = new GenericWriterCreator<MissionTypeSupportAdapter, Mission>();


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
            //for (int i = 0; i < 100; i++)
            //{
                var msg = new Mission()
                {
                    Key = 0,
                    Name = $"{0} mission",
                    Description = "Fire Command mission ",
                    Status = "to fire"
                };
                await _publisher.Publish(MissionTopic, msg);
                Console.WriteLine($"C4I SEND Mission {msg.Name}");
            //    await Task.Delay(100);
            //}
        }

        private async void OnMessageArrived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} C4I RCV Location {((Location)e).Key}");
            await ExportMission();
        }
    }
}
