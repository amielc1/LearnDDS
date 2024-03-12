using System;

namespace MissionSubscriber.Interface;

public interface IImporter
{
    event EventHandler<object> DataReceived;
    void Start();
    void Stop();
}