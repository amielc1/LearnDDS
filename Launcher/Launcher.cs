using DDSService;
using DDSService.Configuration;
using DDSService.Imp.Adapters;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;
using OpenDDSharp.DDS;
namespace CombatSystemDemo.Devices;

public class Launcher
{
    private readonly IDataReaderCreator _reader;
    private readonly IDdsService _ddsService;
    private readonly DdsConfiguration _config;
    private readonly ISubscriber _subscriber;
     
    public Launcher()
    {
        IGenericDataReader<Mission> ReaderFactory(DataReader reader) => new MissionDataReaderAdapter(reader);
        _reader = new GenericReaderCreator<MissionTypeSupportAdapter, Mission>(ReaderFactory);
         
        _config = new DdsConfiguration
        {
            Topic = "MissionTopic"
        };
      
        _ddsService = new OpenDdsService(_config);
        _subscriber = new DdsSubscriber(_ddsService, _reader);
    }

    public async Task Import()
    { 
        await _subscriber.Subscribe(_config.Topic, OnMessageArrived);
        Console.WriteLine($"Launcher Subscribe {_config.Topic}");
    }

    public void OnMessageArrived(object sender, object e)
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Launcher RCV Mission  {((Mission)e).Name} from topic {_config.Topic}");
    }
     
}