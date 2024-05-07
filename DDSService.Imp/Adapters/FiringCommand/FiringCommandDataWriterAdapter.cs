using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class FiringCommandDataWriterAdapter : IGenericDataWriter
{
    private readonly FiringCommandDataWriter _writer;

    public FiringCommandDataWriterAdapter(DataWriter writer)
    {
        _writer = new FiringCommandDataWriter(writer);
    }

    public ReturnCode Write(object data)
    {
        _writer.Write((FiringCommand)data);
        return ReturnCode.Ok;
    }
}