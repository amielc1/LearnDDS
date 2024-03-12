using System;
using MissionSubscriber.Interface;

namespace MissionSubscriber;

public class DDSImporter : IImporter
{
    private IDataReaderCreator _creator;
    private IOpenDdsService _service;
    public event EventHandler<object> DataReceived = delegate { };

    public DDSImporter()
    {
        _creator = new MissionReaderCreator();
        _service = new OpenDdsService(_creator);
    }
    public void Start()
    {
        _service.Subscribe();
        _service.DataReceived += (s, e) => DataReceived(s, e);

    }

    public void Stop()
    {
       _service.UnSubscribe();
    }
}