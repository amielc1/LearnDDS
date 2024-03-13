using OpenDDSharp.DDS;

namespace DDSService.Interface;

public interface IDataReaderCreator
{
    event EventHandler<object> DataReceived;
    DataReader Subscribe(DomainParticipant participant, string topic);
    void UnSubscribe();
}