using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Adapters;

public class MissionDataWriterAdapter : IGenericDataWriter<Mission>
{
    private MissionDataWriter _writer;

    public MissionDataWriterAdapter(DataWriter writer)
    {
        _writer = new MissionDataWriter(writer);
    }

    public ReturnCode Write(Mission data)
    {
        _writer.Write(data);
        return ReturnCode.Ok;
    }
}