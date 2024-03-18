using MissionModule;
using OpenDDSharp.DDS;
using DDSService.Interface;

namespace DDSService.Imp.Adapters;

public class LocationTypeSupportAdapter : ITypeSupport
{
    private readonly LocationTypeSupport _typeSupport = new();

    public string GetTypeName() => _typeSupport.GetTypeName();

    public ReturnCode RegisterType(DomainParticipant participant, string typeName) =>
        _typeSupport.RegisterType(participant, typeName);
}