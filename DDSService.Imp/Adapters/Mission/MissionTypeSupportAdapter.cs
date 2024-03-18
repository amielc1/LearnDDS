using MissionModule;
using OpenDDSharp.DDS;
using DDSService.Interface;

namespace DDSService.Imp.Adapters
{
    public class MissionTypeSupportAdapter : ITypeSupport
    {
        private readonly MissionTypeSupport _typeSupport = new();

        public string GetTypeName() => _typeSupport.GetTypeName();

        public ReturnCode RegisterType(DomainParticipant participant, string typeName) =>
            _typeSupport.RegisterType(participant, typeName);
    }
}