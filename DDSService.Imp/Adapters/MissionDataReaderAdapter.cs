using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;
 
public class MissionDataReaderAdapter : IGenericDataReader<Mission>
{
    private readonly MissionDataReader _missionDataReader;

    public MissionDataReaderAdapter(DataReader reader)
    {
        _missionDataReader = new MissionDataReader(reader); 
    }

    public ReturnCode Take(List<Mission> dataValues, List<SampleInfo> sampleInfos, EventHandler<Mission> DataReceived)
    {
        var receivedData = new List<Mission>();
        var receivedInfo = new List<SampleInfo>();
        var result = _missionDataReader.Take(receivedData, receivedInfo);

        if (result == ReturnCode.Ok)
        {
            foreach (var info in receivedInfo)
            {
                if (!info.ValidData) continue;
                var index = receivedInfo.IndexOf(info);
                var mission = receivedData[index];
                DataReceived.Invoke(this,mission);
            }
        }
        else
        {
            Console.WriteLine($"No data available or error in reading data: {result}");
        }
        return ReturnCode.Ok;
    }

       
}