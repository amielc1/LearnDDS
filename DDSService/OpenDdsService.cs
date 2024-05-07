using DDSService.Configuration;
using DDSService.Interface;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace DDSService;

public delegate IGenericDataReader DataReaderFactory(DataReader reader);

public delegate IGenericDataWriter DataWriterFactory(DataWriter writer);

public class DdsService : IDdsService
{
    private readonly DdsConfiguration _ddsConfiguration;
    private DomainParticipant _participant;
    private readonly DomainParticipantFactory _dpf;
    private static DdsService instance = null;
    private static readonly object padlock = new object();

    private DdsService(DdsConfiguration ddsConfiguration)
    {
        _ddsConfiguration = ddsConfiguration;
        Ace.Init();
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory(_ddsConfiguration.DCPSConfigFile, _ddsConfiguration.rtps);
    }

    public static DdsService GetInstance(DdsConfiguration config)
    {
        if (instance == null)
        {
            lock (padlock)
            {
                if (instance == null && config != null) // Ensure config is not null
                {
                    instance = new DdsService(config);
                }
            }
        }
        return instance;
    }

    public DomainParticipant CreateParticipant()
    {
        try
        {
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