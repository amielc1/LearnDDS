using System;

namespace DDSService.Interface;

public interface IOpenDdsService : IDisposable
{
    event EventHandler<object> DataReceived;
    void Subscribe();
    void UnSubscribe();
}