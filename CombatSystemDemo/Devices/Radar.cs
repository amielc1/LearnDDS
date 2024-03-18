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
        private static int counter = 0;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly IPublisher _publisher;
         
        public Radar()
        {

            DataWriterFactory.Register(writer => new MissionDataWriterAdapter(writer));
            IDataWriterCreator creator = new GenericWriterCreator<MissionTypeSupportAdapter, Mission>();

            _config = new DdsConfiguration
            {
                Topic = "MissionTopic"
            };

            _ddsService = new OpenDdsService(_config);
            _publisher = new DdsPublisher(_ddsService, creator);
        }

        public async Task Export()
        {
            for (int i = 0; i < 100; i++)
            {
                var msg = new Mission()
                {
                    Key = counter++, Name = $"{counter} mission", Description = "Fire Command mission ",
                    Status = "to fire"
                };
                await _publisher.Publish(_config.Topic, msg);
                Console.WriteLine($"Radar SEND {msg.Name}");
                await Task.Delay(100);
            }
        }   
    }
}
