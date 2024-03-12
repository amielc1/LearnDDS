using System;

namespace MissionSubscriber.Interface;

public interface IOpenDdsService : IDisposable
{
    event EventHandler<object> DataReceived;
    void Subscribe();
    void UnSubscribe();
}