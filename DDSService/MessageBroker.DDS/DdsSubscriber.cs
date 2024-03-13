
using DDSService.Interface;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace DDSService.MessageBroker.DDS;

public class DdsSubscriber : ISubscriber
{
    private readonly IDdsService _ddsService;
    private readonly IDataReaderCreator _dataReaderCreator;
    private DomainParticipant _participant;
    public DdsSubscriber(IDdsService ddsService, IDataReaderCreator dataReaderCreator)
    {
        _ddsService = ddsService;
        _dataReaderCreator = dataReaderCreator;
        _participant = _ddsService.CreateParticipant();
    }

    public Task Subscribe(string topic, EventHandler<object> onMessageArrived)
    {
        try
        {
            _dataReaderCreator.Subscribe(_participant, topic);
            _dataReaderCreator.DataReceived += (s, e) => onMessageArrived(s, e);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }

    public Task UnSubscribe()
    {
        try
        {
            _dataReaderCreator.UnSubscribe();
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
}