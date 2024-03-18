using MissionModule;
using OpenDDSharp.DDS;
using DDSService.Interface;

namespace DDSService.Imp.Adapters
{
    public class MissionTypeSupportAdapter : ITypeSupport
    {
        private readonly MissionTypeSupport _missionTypeSupport = new MissionTypeSupport();

        public string GetTypeName() => _missionTypeSupport.GetTypeName();

        public ReturnCode RegisterType(DomainParticipant participant, string typeName) =>
            _missionTypeSupport.RegisterType(participant, typeName);
    }
}
