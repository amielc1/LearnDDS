using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;
using OpenDDSharp.DDS;
using System;
using System.Threading.Tasks;

namespace CombatSystemDemo.Devices
{
    public class C4I
    {
        static int counter = 0; 
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly IPublisher _missionpublisher;
        private readonly IPublisher _locationpublisher;
        private readonly ISubscriber _fireSubscriber;

        private const string MissionTopic = nameof(MissionTopic);
        private const string LocationTopic = nameof(LocationTopic);
        private const string FireTopic = nameof(FireTopic);

        public C4I()
        {
            IGenericDataWriter MissionWriterFactory(DataWriter writer) => new MissionDataWriterAdapter(writer);
            IDataWriterCreator _missionwriter = new GenericWriterCreator<MissionTypeSupportAdapter>(MissionWriterFactory);
            IGenericDataWriter LocationWriterFactory(DataWriter writer) => new LocationDataWriterAdapter(writer);
            IDataWriterCreator _locationwriter = new GenericWriterCreator<LocationTypeSupportAdapter>(LocationWriterFactory);

            IDataReaderCreator fireReader = new GenericReaderCreator<FiringCommandTypeSupportAdapter>(reader => new FiringCommandDataReaderAdapter(reader));

            _config = new DdsConfiguration();
            _ddsService = DdsService.GetInstance(_config);
            var participant = _ddsService.CreateParticipant();
            _missionpublisher = new DdsPublisher(participant, _missionwriter);
            _locationpublisher = new DdsPublisher(participant, _locationwriter);
            _fireSubscriber = new DdsSubscriber(participant, fireReader);

        }

        public async Task Import()
        {
            await _fireSubscriber.Subscribe(FireTopic, OnFireArrived);
        }

        private void OnFireArrived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Launcher RCV Fire  {((MissionModule.FiringCommand)e).Key} ");
        }

        public async Task ExportMission()
        {
            var msg = new MissionModule.Mission()
            {
                Key = counter++,
                Name = $"{counter} mission",
                Description = "Fire Command mission ",
                Status = "to fire"
            };
            await _missionpublisher.Publish(MissionTopic, msg);
            await Task.Delay(100);
            Console.WriteLine($"C4I SEND Mission {msg.Name} to topic {MissionTopic}");

        }
        public async Task ExportLocation()
        {
            var msg = new Location()
            {
                Key = counter,
                Latitude = counter,
                Longtitude = counter,
                Altitude = counter,
            };
            await _locationpublisher.Publish(LocationTopic, msg);
            await Task.Delay(100);
            Console.WriteLine($"C4I SEND Location {msg.Key} to topic {LocationTopic}");
        }
    } 
}

