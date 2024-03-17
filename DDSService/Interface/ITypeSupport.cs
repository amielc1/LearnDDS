using OpenDDSharp.DDS;

namespace DDSService.Interface
{
    public interface ITypeSupport
    {
        string GetTypeName();
        ReturnCode RegisterType(DomainParticipant participant, string typeName);
    }
}
