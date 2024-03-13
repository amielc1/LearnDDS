namespace MessageBroker.Core.Interfaces;

public interface IPublisher
{
    Task Publish(string topic, object data);
}