using OpenDDSharp.DDS;
using System;

namespace MissionSubscriber.Interface;

public interface IDataReaderCreator
{
    event EventHandler<object> DataReceived;
    DataReader CreateDataReader(DomainParticipant participant, string topic);
    void UnSubscribe();
}