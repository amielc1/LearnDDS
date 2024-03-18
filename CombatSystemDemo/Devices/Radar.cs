using System;
using System.Threading.Tasks;
using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;

namespace CombatSystemDemo.Devices
{
    public class Radar
    { 
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly IPublisher _publisher;
         
        public Radar()
        {

            DataWriterFactory.Register(writer => new LocationDataWriterAdapter(writer));
            IDataWriterCreator creator = new GenericWriterCreator<LocationTypeSupportAdapter, Location>();

            _config = new DdsConfiguration
            {
                Topic = "LocationTopic"
            };

            _ddsService = new OpenDdsService(_config);
            _publisher = new DdsPublisher(_ddsService, creator);
        }

        public async Task Export()
        {
            for (int i = 0; i < 100; i++)
            {
                var msg = new Location()
                {
                     Key = i,
                     Latitude = i,
                     Longtitude = i,
                     Altitude = i,
                };
                await _publisher.Publish(_config.Topic, msg);
                Console.WriteLine($"Radar SEND Location {msg.Key}");
                await Task.Delay(100);
            }
        }   
    }
}
