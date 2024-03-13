using OpenDDSharp.DDS;

namespace DDSService.Interface;

public interface IDataWriterCreator
{
    DataWriter CreateWriter(DomainParticipant participant, string topic);
    Task Publish(object data);
}