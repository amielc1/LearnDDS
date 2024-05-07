using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class LocationDataWriterAdapter : IGenericDataWriter
{
    private readonly LocationDataWriter _writer;

    public LocationDataWriterAdapter(DataWriter writer)
    {
        _writer = new LocationDataWriter(writer);
    }

    public ReturnCode Write(object data)
    {
        _writer.Write((Location)data);
        return ReturnCode.Ok;
    }
}