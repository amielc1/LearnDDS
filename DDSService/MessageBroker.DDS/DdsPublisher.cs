using DDSService.Interface;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace DDSService.MessageBroker.DDS;

public class DdsPublisher : IPublisher
{
    private readonly IDdsService _ddsService;
    private readonly DomainParticipant _participant;
    private readonly IDataWriterCreator _dataWriterCreator;


    public DdsPublisher(IDdsService ddsService, IDataWriterCreator dataWriterCreator)
    {
        _ddsService = ddsService;
        _dataWriterCreator = dataWriterCreator;
        _participant = _ddsService.CreateParticipant();

    }
    public Task Publish(string topic, object data)
    {
        try
        {
            _dataWriterCreator.CreateWriter(_participant, topic);
            _dataWriterCreator.Publish(data);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
}