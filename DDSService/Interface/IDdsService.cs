using OpenDDSharp.DDS;

namespace DDSService.Interface;

public interface IDdsService : IDisposable
{
    DomainParticipant CreateParticipant();
}