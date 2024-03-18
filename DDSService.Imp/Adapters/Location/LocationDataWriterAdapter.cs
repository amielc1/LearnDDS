using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class LocationDataWriterAdapter : IGenericDataWriter<Location>
{
    private readonly LocationDataWriter _writer;

    public LocationDataWriterAdapter(DataWriter writer)
    {
        _writer = new LocationDataWriter(writer);
    }

    public ReturnCode Write(Location data)
    {
        _writer.Write(data);
        return ReturnCode.Ok;
    }
}