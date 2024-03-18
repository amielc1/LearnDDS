using OpenDDSharp.DDS;

namespace DDSService.Interface
{
    public interface IGenericDataWriter<T>
    {
        ReturnCode Write(T data);
    }
}

