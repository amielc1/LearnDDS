using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MissionModule;
using MissionSubscriber.Interface;
using OpenDDSharp.DDS;

namespace MissionSubscriber;

public class MissionReaderCreator : IDataReaderCreator
{
    public event EventHandler<object> DataReceived = delegate { };
    private Subscriber _subscriber;
    private DataReader _dataReader;
    private MissionDataReader _missionDataReader;
    private Topic _topic;
    private DomainParticipant _participant;
    public DataReader CreateDataReader(DomainParticipant participant, string topic)
    {
        _participant = participant;
        MissionTypeSupport missionTypeSupport = new();
        var missionTypeName = missionTypeSupport.GetTypeName();
        ReturnCode result = missionTypeSupport.RegisterType(_participant, missionTypeName);
        Console.WriteLine($"Register participant {_participant.DomainId}, with MissionType {missionTypeName} ");
        if (result != ReturnCode.Ok)
        {
            throw new Exception("Could not register type: " + result.ToString());
        }

        Console.WriteLine($"Create Topic {topic} with participant {_participant.DomainId} and MissionType {missionTypeName} ");
        var _topic = _participant.CreateTopic(topic, missionTypeName);
        if (_topic == null)
        {
            throw new Exception("Could not create the message topic");
        }

        Console.WriteLine($"Create Subscriber on participant {_participant.DomainId} ");
        _subscriber = _participant.CreateSubscriber();
        if (_subscriber == null)
        {
            throw new Exception("Could not create the subscriber");
        }

        Console.WriteLine($"Create DataReader on participant {participant.DomainId} ");
        _dataReader = _subscriber.CreateDataReader(_topic);
        if (_dataReader == null)
        {
            throw new Exception("Could not create the  DataReader");
        }

        Console.WriteLine($"Wrap DataReader with MissionDataReader helper class");
        _missionDataReader = new(_dataReader);

        Task.Factory.StartNew(() => { WaitForEvents(_missionDataReader); });

        return _missionDataReader;
    }

    public void UnSubscribe()
    {
        _participant.DeleteSubscriber(_subscriber);
    }

    private void WaitForEvents(MissionDataReader missionDataReader)
    {
        while (true)
        {
            StatusMask mask = missionDataReader.StatusChanges;
            if ((mask & StatusKind.DataAvailableStatus) != 0)
            {
                List<Mission> receivedData = new();
                List<SampleInfo> receivedInfo = new();
                var result = missionDataReader.Take(receivedData, receivedInfo);

                if (result == ReturnCode.Ok)
                {
                    bool messageReceived = false;
                    for (int i = 0; i < receivedData.Count; i++)
                    {
                        if (receivedInfo[i].ValidData)
                        {
                            var mission = new Mission()
                            {
                                Key = receivedData[i].Key,
                                Name = receivedData[i].Name,
                                Description = receivedData[i].Description,
                                Status = receivedData[i].Status
                            };
                            DataReceived.Invoke(this, mission);
                        }
                    }
                }
            }

            System.Threading.Thread.Sleep(100);
        }
    }
}