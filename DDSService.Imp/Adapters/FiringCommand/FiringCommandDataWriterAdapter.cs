using DDSService.Interface;
using MissionModule;
using OpenDDSharp.DDS;

namespace DDSService.Imp.Adapters;

public class FiringCommandDataWriterAdapter : IGenericDataWriter<FiringCommand>
{
    private readonly FiringCommandDataWriter _writer;

    public FiringCommandDataWriterAdapter(DataWriter writer)
    {
        _writer = new FiringCommandDataWriter(writer);
    }

    public ReturnCode Write(FiringCommand data)
    {
        _writer.Write(data);
        return ReturnCode.Ok;
    }
}