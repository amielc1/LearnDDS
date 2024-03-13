using DDSService.Configuration;
using DDSService.Interface;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace DDSService;

public class OpenDdsService : IDdsService
{
    private readonly DdsConfiguration _ddsConfiguration;
    private DomainParticipant _participant;
    private DomainParticipantFactory _dpf;

    public OpenDdsService(DdsConfiguration ddsConfiguration)
    {
        _ddsConfiguration = ddsConfiguration;
        Ace.Init();
    }

    public DomainParticipant CreateParticipant()
    {
        try
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory(_ddsConfiguration.DCPSConfigFile, _ddsConfiguration.rtps);
            Console.WriteLine($"Create DomainParticipant {_ddsConfiguration.DomainId}");
            _participant = _dpf.CreateParticipant(_ddsConfiguration.DomainId);
            if (_participant == null)
            {
                throw new Exception("Could not create the participant");
            }

            return _participant;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }

    public void Dispose()
    {
        try
        {
            _participant.DeleteContainedEntities();
            _dpf.DeleteParticipant(_participant);
            ParticipantService.Instance.Shutdown();
            Ace.Fini();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
}