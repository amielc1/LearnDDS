using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class MissionDataReaderAdapter : IGenericDataReader
{
    private readonly MissionDataReader _reader; 
    public MissionDataReaderAdapter(DataReader reader)
    {
        _reader = new MissionDataReader(reader); 
    }

    public ReturnCode Take(EventHandler<object> DataReceived)
    {
        var receivedData = new List<Mission>();
        var receivedInfo = new List<SampleInfo>();
        var result = _reader.Take(receivedData, receivedInfo);

        if (result == ReturnCode.Ok)
        {
            foreach (var info in receivedInfo)
            {
                if (!info.ValidData) continue;
                var index = receivedInfo.IndexOf(info);
                var mission = receivedData[index];
                DataReceived(this, mission);
            } 
        }
        else
        {
            Console.WriteLine($"No data available or error in reading data: {result}");
        }
        return ReturnCode.Ok;
    }

       
}