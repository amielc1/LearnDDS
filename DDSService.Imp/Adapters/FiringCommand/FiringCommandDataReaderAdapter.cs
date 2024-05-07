using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class FiringCommandDataReaderAdapter : IGenericDataReader
{
    private readonly FiringCommandDataReader _reader;

    public FiringCommandDataReaderAdapter(DataReader reader)
    {
        _reader = new FiringCommandDataReader(reader);
    }

    public ReturnCode Take(EventHandler<object> DataReceived)
    {
        var receivedData = new List<FiringCommand>();
        var receivedInfo = new List<SampleInfo>();
        var result = _reader.Take(receivedData, receivedInfo);

        if (result == ReturnCode.Ok)
        {
            foreach (var info in receivedInfo)
            {
                if (!info.ValidData) continue;
                var index = receivedInfo.IndexOf(info);
                var data = receivedData[index];
                DataReceived.Invoke(this, data);
            }
        }
        else
        {
            Console.WriteLine($"No data available or error in reading data: {result}");
        }
        return ReturnCode.Ok;
    }
}