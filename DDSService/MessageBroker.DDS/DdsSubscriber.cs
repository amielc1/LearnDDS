
using DDSService.Interface;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace DDSService.MessageBroker.DDS;

public class DdsSubscriber : ISubscriber
{
    private readonly IDataReaderCreator _dataReaderCreator;
    private readonly DomainParticipant _participant;
    public DdsSubscriber(DomainParticipant participant, IDataReaderCreator dataReaderCreator)
    {
        _dataReaderCreator = dataReaderCreator;
        _participant = participant;
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