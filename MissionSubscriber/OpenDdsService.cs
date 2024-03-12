using System;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
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
    private string _topic;

    public OpenDdsService(IDataReaderCreator dataReaderCreator)
    {
        _dataReaderCreator = dataReaderCreator;
        //read topic from configuration ;  
        _topic = "MissionTopic";
        Init();
    }

    private DomainParticipant CreateParticipant()
    {
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");
        Console.WriteLine("Create DomainParticipant (42)");
        DomainParticipant participant = _dpf.CreateParticipant(42);
        if (participant == null)
        {
            throw new Exception("Could not create the participant");
        }
        return participant;
    }

    private void Init()
    {
        Ace.Init();
        _participant = CreateParticipant(); 
    }

    public void Dispose()
    {
        _participant.DeleteContainedEntities();
        _dpf.DeleteParticipant(_participant);
        ParticipantService.Instance.Shutdown();
    }
    public void Subscribe()
    {
        _dataReader = _dataReaderCreator.CreateDataReader(_participant, _topic);
        _dataReaderCreator.DataReceived += (s, e) => DataReceived(s, e);
    }

    public void UnSubscribe()
    {
        _dataReaderCreator.UnSubscribe();
    }
}