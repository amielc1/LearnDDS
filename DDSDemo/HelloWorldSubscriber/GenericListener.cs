using OpenDDSharp.DDS;
using System;
using HelloWorld;
using System.Collections.Generic;

namespace HelloWorldSubscriber;

public class GenericListener : DataReaderListener
{
    private readonly Action<string> _callback;
    private MessageDataReader _messageReader;
    public event EventHandler<string> DataReceived = delegate { };

    protected override void OnDataAvailable(DataReader reader)
    {
        List<Message> receivedData = new();
        List<SampleInfo> receivedInfo = new();
        if (_messageReader.Take(receivedData, receivedInfo) != ReturnCode.Ok) return;
        
        for (int i = 0; i < receivedData.Count; i++)
        {
            if (receivedInfo[i].ValidData)
            {
                DataReceived?.Invoke(this, receivedData[i].Content);
            }
        }
    }

    protected override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
    {
        Console.WriteLine($"OnRequestedDeadlineMissed {status}");
    }

    protected override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
    {
        Console.WriteLine($"OnRequestedIncompatibleQos {status}");
    }

    protected override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
    {
        Console.WriteLine($"OnSampleRejected {status}");
    }

    protected override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
    {
        Console.WriteLine($"OnLivelinessChanged {status}");
    }

    protected override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
    {
        _messageReader = new MessageDataReader(reader);
        Console.WriteLine($"OnSubscriptionMatched {status}");
    }

    protected override void OnSampleLost(DataReader reader, SampleLostStatus status)
    {
        Console.WriteLine($"OnSampleLost {status}");
    }
}