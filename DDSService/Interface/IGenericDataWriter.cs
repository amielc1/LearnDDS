using OpenDDSharp.DDS;

namespace DDSService.Interface
{
    public interface IGenericDataWriter
    {
        ReturnCode Write(object data);
    }
}

