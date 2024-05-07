using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class MissionDataWriterAdapter : IGenericDataWriter
{
    private readonly MissionDataWriter _writer;

    public MissionDataWriterAdapter(DataWriter writer)
    {
        _writer = new MissionDataWriter(writer);
    }

    public ReturnCode Write(object data)
    {
        _writer.Write((Mission)data);
        return ReturnCode.Ok;
    }
}