using OpenDDSharp.DDS;

namespace DDSService.Interface;

public interface IGenericDataReader
{
    ReturnCode Take(EventHandler<object> dataReceived);
}