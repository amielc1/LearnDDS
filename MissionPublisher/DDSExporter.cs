using DDSService;
using DDSService.Configuration;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;

namespace MissionPublisher;

public class DDSExporter
{
    private readonly IDataWriterCreator _creator;
    private readonly IDdsService _ddsService;
    private readonly DdsConfiguration _config;
    private readonly IPublisher _publisher;
    public DDSExporter()
    {
        _creator = new MissionWriterCreator();
        _config = new DdsConfiguration();
        _ddsService = new OpenDdsService(_config);
        _publisher = new DdsPublisher(_ddsService, _creator);
    }

    public void Export(object data)
    {
        _publisher.Publish(_config.Topic, data);
    }
}