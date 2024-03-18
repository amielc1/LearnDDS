using MissionModule;
using OpenDDSharp.DDS;
using DDSService.Interface;

namespace DDSService.Imp.Adapters;

public class LocationTypeSupportAdapter : ITypeSupport
{
    private LocationTypeSupport _locationTypeSupport = new LocationTypeSupport();

    public string GetTypeName() => _locationTypeSupport.GetTypeName();

    public ReturnCode RegisterType(DomainParticipant participant, string typeName) =>
        _locationTypeSupport.RegisterType(participant, typeName);
}