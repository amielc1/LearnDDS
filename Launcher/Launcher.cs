using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;

namespace CombatSystemDemo.Devices;

public class Launcher
{
    private readonly DdsConfiguration _config;
    private readonly ISubscriber _subscriber;

    public Launcher()
    {
        IDataReaderCreator missionReader = new GenericReaderCreator<MissionTypeSupportAdapter>(reader => new MissionDataReaderAdapter(reader));

        _config = new DdsConfiguration
        {
            Topic = "MissionTopic"
        };

        IDdsService ddsService = DdsService.GetInstance(_config);
        _subscriber = new DdsSubscriber(ddsService.CreateParticipant(), missionReader);
    }

    public async Task Import()
    {
        await _subscriber.Subscribe(_config.Topic, OnMessageArrived);
        Console.WriteLine($"Launcher Subscribe {_config.Topic}");
    }

    public void OnMessageArrived(object sender, object e)
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Launcher RCV Mission  {(e)} from topic {_config.Topic}");
    }
}