using DDSService.Interface;
using MessageBroker.Core.Interfaces;
using OpenDDSharp.DDS;

namespace DDSService.MessageBroker.DDS;

public class DdsSubscriber : ISubscriber
{
    private readonly IDataReaderCreator _dataReaderCreator;
    private readonly DomainParticipant _participant;
    private EventHandler<object>? _dataReceivedHandler;

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
            _dataReceivedHandler = (s, e) => onMessageArrived(s, e);
            _dataReaderCreator.DataReceived += _dataReceivedHandler;
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
            if (_dataReceivedHandler != null)
            {
                _dataReaderCreator.DataReceived -= _dataReceivedHandler;
                _dataReceivedHandler = null;
            }
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
