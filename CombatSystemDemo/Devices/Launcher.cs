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

namespace CombatSystemDemo.Devices;

public class Launcher
{ 
    private readonly IDataReaderCreator _creator;
    private readonly IDdsService _ddsService;
    private readonly DdsConfiguration _config;
    private readonly ISubscriber _subscriber;

 
    public Launcher()
    {

        IGenericDataReader<Mission> Factory(DataReader reader) => new MissionDataReaderAdapter(reader);
        var readerCreator = new GenericReaderCreator<MissionTypeSupportAdapter, Mission>(Factory);
             
        _creator = readerCreator;
        _config = new DdsConfiguration
        {
            Topic = "MissionTopic"
        };
        _ddsService = new OpenDdsService(_config);
        _subscriber = new DdsSubscriber(_ddsService, _creator);
    }

    public async Task Import()
    {
         await _subscriber.Subscribe(_config.Topic, OnMessageArrived);
    }

    public void OnMessageArrived(object sender, object e)
    {
        Console.WriteLine($"Launcher RCV {DateTime.Now.ToLongTimeString()}  {((Mission)e).Name}");
    }
}