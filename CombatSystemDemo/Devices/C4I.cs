using System;
using System.Threading.Tasks;
using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace CombatSystemDemo.Devices
{
    public class C4I
    {
        static int counter = 0;
        private readonly IDataWriterCreator _writer;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly IPublisher _publisher;


        private const string MissionTopic = "MissionTopic";

        public C4I()
        {
            IGenericDataWriter WriterFactory(DataWriter writer) => new MissionDataWriterAdapter(writer);
            _writer = new GenericWriterCreator<MissionTypeSupportAdapter>(WriterFactory);

 

            _config = new DdsConfiguration
            {
                Topic = "LocationTopic"
            };
            _ddsService = new OpenDdsService(_config);
            _publisher = new DdsPublisher(_ddsService, _writer);
        }

        public async Task Import()
        {
            Console.WriteLine($"C4I Subscribe to {_config.Topic}");
        }

        public async Task ExportMission()
        { 
                 var msg = new  MissionModule.Mission()
                {
                    Key = counter++,
                    Name = $"{counter} mission",
                    Description = "Fire Command mission ",
                    Status = "to fire"
                };
                await _publisher.Publish(MissionTopic, msg);
                await Task.Delay(100);
                Console.WriteLine($"C4I SEND Mission {msg.Name} to topic {MissionTopic}");     }

       
    }
}
