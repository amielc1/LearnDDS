using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;
using OpenDDSharp.DDS;

namespace console1.Devices
{
    public class Radar
    { 
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly IPublisher _publisher;
         
        public Radar()
        {

            IGenericDataWriter WriterFactory(DataWriter writer) => new LocationDataWriterAdapter(writer); 
            IDataWriterCreator creator = new GenericWriterCreator<LocationTypeSupportAdapter>(WriterFactory);

            _config = new DdsConfiguration
            {
                Topic = "LocationTopic"
            };

            _ddsService = DdsService.GetInstance(_config);
            _publisher = new DdsPublisher(_ddsService.CreateParticipant(), creator);
        }

        public async Task Export()
        {
            for (int i = 0; i < 100000; i++)
            {
                var msg = new Location()
                {
                     Key = i,
                     Latitude = i,
                     Longtitude = i,
                     Altitude = i,
                };
                await _publisher.Publish(_config.Topic, msg);
                Console.WriteLine($"Radar SEND Location {msg.Key} to topic {_config.Topic}");
                await Task.Delay(100);
            }
        }   
    }
}
