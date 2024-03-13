namespace MessageBroker.Core.Interfaces;

public interface ISubscriber
{
    Task Subscribe(string topic, EventHandler<object> onMessageArrived);
    Task UnSubscribe();
}