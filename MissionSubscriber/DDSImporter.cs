using System;
using DDSService;
using DDSService.Configuration;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionSubscriber.Interface;

namespace MissionSubscriber;

public class DDSImporter : IImporter
{
    private readonly IDataReaderCreator _creator;
    private readonly IDdsService _ddsService;
    private readonly DdsConfiguration _config;
    private readonly ISubscriber _subscriber;
    public event EventHandler<object> DataReceived = delegate { };

    public DDSImporter()
    {
        _creator = new MissionReaderCreator();
        _config = new DdsConfiguration();
        _ddsService = new OpenDdsService(_config);
        _subscriber = new DdsSubscriber(_ddsService, _creator);
    }
    public void Start()
    {
        _subscriber.Subscribe(_config.Topic, (s, e) => DataReceived(s, e));
    } 

    public void Stop()
    {
        _subscriber.UnSubscribe();
    }
}