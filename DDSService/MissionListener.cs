using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService;

public class MissionListener : DataReaderListener
{
    public event EventHandler<Mission> DataReceived = delegate { };
    private MissionDataReader missionDataReader = null;

    private void ProcessDataEvents()
    {
        var receivedData = new List<Mission>();
        var receivedInfo = new List<SampleInfo>();
        var result = missionDataReader.Take(receivedData, receivedInfo);

        if (result == ReturnCode.Ok)
        {
            foreach (var info in receivedInfo)
            {
                if (!info.ValidData) continue;
                var index = receivedInfo.IndexOf(info);
                var mission = receivedData[index];
                DataReceived?.Invoke(this, mission);
            }
        }
        else
        {
            Console.WriteLine($"No data available or error in reading data: {result}");
        }
    }
    protected override void OnDataAvailable(DataReader reader)
    {
        ProcessDataEvents();
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
        Console.WriteLine($"OnSubscriptionMatched {status}");
        missionDataReader = new MissionDataReader(reader);
    }

    protected override void OnSampleLost(DataReader reader, SampleLostStatus status)
    {
        Console.WriteLine($"OnSampleLost {status}");
    }
}