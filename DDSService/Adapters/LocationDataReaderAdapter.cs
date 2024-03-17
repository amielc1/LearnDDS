using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Adapters
{
    public class LocationDataReaderAdapter : IGenericDataReader<Location>
    {
        private LocationDataReader _locationDataReader;

        public LocationDataReaderAdapter(DataReader reader)
        {
            _locationDataReader = (LocationDataReader)reader; // Assume safe casting based on your system's design
        }

        public ReturnCode Take(List<Location> dataValues, List<SampleInfo> sampleInfos, EventHandler<Location> DataReceived)
        {
            var receivedData = new List<Location>();
            var receivedInfo = new List<SampleInfo>();
            var result = _locationDataReader.Take(receivedData, receivedInfo);

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
}
