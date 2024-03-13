using System;
using MissionSubscriber.Configuration;
using MissionSubscriber.Interface;

namespace MissionSubscriber;

public class DDSImporter : IImporter
{
    private readonly IDataReaderCreator _creator;
    private readonly IOpenDdsService _openDdsService;
    private readonly DdsConfiguration _config;
    public event EventHandler<object> DataReceived = delegate { };

    public DDSImporter()
    {
        _creator = new MissionReaderCreator();
        _config = new DdsConfiguration();
        _openDdsService = new OpenDdsService(_creator, _config);
    }
    public void Start()
    {
        _openDdsService.Subscribe();
        _openDdsService.DataReceived += OnDataReceived;
    }

    public void Stop()
    {
        _openDdsService.UnSubscribe();
        _openDdsService.DataReceived -= OnDataReceived;

    }

    private void OnDataReceived(object sender, object eventArgs)
    {
        DataReceived?.Invoke(sender, eventArgs);
    }
}