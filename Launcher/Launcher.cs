using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace CombatSystemDemo.Devices;

public class Launcher
{
    static int counter = 2000;
    private readonly DdsConfiguration _config;
    private readonly ISubscriber _missionSubscriber;
    private readonly ISubscriber _locationSubscriber;
    private readonly IPublisher _firePublisher;
    private const string MissionTopic = nameof(MissionTopic);
    private const string LocationTopic = nameof(LocationTopic);
    private const string FireTopic = nameof(FireTopic);

    public Launcher()
    {
        IGenericDataWriter FireWriterFactory(DataWriter writer) => new FiringCommandDataWriterAdapter(writer);
        IDataWriterCreator _firewriter = new GenericWriterCreator<FiringCommandTypeSupportAdapter>(FireWriterFactory);
        IDataReaderCreator missionReader = new GenericReaderCreator<MissionTypeSupportAdapter>(reader => new MissionDataReaderAdapter(reader));
        IDataReaderCreator locationReader = new GenericReaderCreator<LocationTypeSupportAdapter>(reader => new LocationDataReaderAdapter(reader));
        _config = new DdsConfiguration();
        IDdsService ddsService = DdsService.GetInstance(_config);
        var participant = ddsService.CreateParticipant();
        _missionSubscriber = new DdsSubscriber(participant, missionReader);
        _locationSubscriber = new DdsSubscriber(participant, locationReader);
        _firePublisher = new DdsPublisher(participant, _firewriter);

    }

    public async Task Import()
    {
        await _missionSubscriber.Subscribe(MissionTopic, OnMissionArrived);
        await _locationSubscriber.Subscribe(LocationTopic, OnLocationArrived);
        await SendFire();
    }

    public async Task SendFire()
    {
        while (true)
        {
            var msg = new MissionModule.FiringCommand()
            {
                Key = counter++,
                TargetId = counter,
                WeaponType = counter,
            };
            await _firePublisher.Publish(FireTopic, msg);
            await Task.Delay(100);
            Console.WriteLine($"Launcher SEND Fire Command {msg.Key} to topic {LocationTopic}");
        }
    }

    private void OnLocationArrived(object? sender, object e)
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Launcher RCV Location  {((MissionModule.Location)e).Key} ");
    }

    public void OnMissionArrived(object sender, object e)
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Launcher RCV Mission  {((MissionModule.Mission)e).Name} ");
    }
}