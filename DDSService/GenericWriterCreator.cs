using System.Collections.Concurrent;
using DDSService.Interface;
using OpenDDSharp.DDS;

namespace DDSService;


public static class DataWriterFactory
{
    private static readonly ConcurrentDictionary<Type, Func<DataWriter, object>> _creators = new();

    public static void Register<TData>(Func<DataWriter, IGenericDataWriter<TData>> creator) where TData : class
    {
        _creators[typeof(TData)] = writer => creator(writer);
    }

    public static IGenericDataWriter<TData> Create<TData>(DataWriter writer) where TData : class
    {
        if (_creators.TryGetValue(typeof(TData), out var creator))
        {
            return (IGenericDataWriter<TData>)creator(writer);
        }
        throw new InvalidOperationException($"No creator registered for type {typeof(TData).FullName}");
    }
}

public class GenericWriterCreator<TTypeSupport, TData> : IDataWriterCreator
    where TTypeSupport : ITypeSupport, new()
    where TData : class
{
    private DomainParticipant _participant;
    private Publisher? _publisher;
    private Topic _topicInstance;
    private IGenericDataWriter<TData> _dataWriter;

    public DataWriter CreateWriter(DomainParticipant participant, string topic)
    {
        if (_dataWriter != null)
            return _dataWriter as DataWriter;

        _participant = participant;
        RegisterType(topic);
        _publisher = CreatePublisher(participant);
        _dataWriter = CreateAndWrapDataWriter(topic);
        return _dataWriter as DataWriter;
    }

    private IGenericDataWriter<TData> CreateAndWrapDataWriter(string topic)
    {
        Console.WriteLine($"Create DataWriter for topic: {topic}");
        var writer = _publisher?.CreateDataWriter(_topicInstance) as DataWriter;
        if (writer == null)
        {
            throw new Exception("Could not create the data writer");
        }

        // Assume there's a factory or method to wrap or create a IGenericDataWriter<TData> instance
        var messageWriter = DataWriterFactory.Create<TData>(writer);
        return messageWriter;
    }

    private static Publisher CreatePublisher(DomainParticipant participant)
    {
        var publisher = participant.CreatePublisher();
        if (publisher == null)
        {
            throw new Exception("Could not create the publisher");
        }
        return publisher;
    }

    private void RegisterType(string topic)
    {
        TTypeSupport typeSupport = new TTypeSupport();
        var typeName = typeSupport.GetTypeName();
        var result = typeSupport.RegisterType(_participant, typeName);

        if (result != ReturnCode.Ok)
        {
            throw new Exception($"Could not register type: {result}");
        }

        _topicInstance = _participant.CreateTopic(topic, typeName);
    }

    public Task Publish(object data)
    {
        try
        {
            var result = _dataWriter.Write((TData)data);
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Failed to write data");
            }
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Log(string message)
    {
        Console.WriteLine(message);
    }

    private void LogError(string message)
    {
        Console.Error.WriteLine(message);
    } 
   
}