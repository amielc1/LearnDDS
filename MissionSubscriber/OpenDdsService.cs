using System;
using MissionSubscriber.Configuration;
using MissionSubscriber.Interface;
using OpenDDSharp;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace MissionSubscriber;

public class OpenDdsService : IOpenDdsService
{
    public event EventHandler<object> DataReceived = delegate { };
    private IDataReaderCreator _dataReaderCreator;
    private DomainParticipant _participant;
    private DomainParticipantFactory _dpf;
    private DataReader _dataReader;
    private DdsConfiguration _ddsConfiguration;

    public OpenDdsService(IDataReaderCreator dataReaderCreator, DdsConfiguration ddsConfiguration)
    {
        _dataReaderCreator = dataReaderCreator;
        _ddsConfiguration = ddsConfiguration;
        Init();
    }

    private DomainParticipant CreateParticipant()
    {

        _dpf = ParticipantService.Instance.GetDomainParticipantFactory(_ddsConfiguration.DCPSConfigFile, _ddsConfiguration.rtps);
        Console.WriteLine($"Create DomainParticipant {_ddsConfiguration.DomainId}");
        DomainParticipant participant = _dpf.CreateParticipant(_ddsConfiguration.DomainId);
        if (participant == null)
        {
            throw new Exception("Could not create the participant");
        }
        return participant;
    }

    private void Init()
    {
        try
        {
            Ace.Init();
            _participant = CreateParticipant();
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
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
    public void Subscribe()
    {
        try
        {
            _dataReader = _dataReaderCreator.CreateDataReader(_participant, _ddsConfiguration.Topic);
            _dataReaderCreator.DataReceived += (s, e) => DataReceived(s, e);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }

    public void UnSubscribe()
    {
        try
        {
            _dataReaderCreator.UnSubscribe();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
}