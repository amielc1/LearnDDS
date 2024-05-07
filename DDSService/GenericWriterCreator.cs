using DDSService.Interface;
using OpenDDSharp.DDS;

namespace DDSService;

public class GenericWriterCreator<TTypeSupport> : IDataWriterCreator
    where TTypeSupport : ITypeSupport, new()
{
    private DomainParticipant _participant;
    private Publisher? _publisher;
    private Topic _topicInstance;
    private IGenericDataWriter _dataWriter;
    private readonly DataWriterFactory _factory;

    public GenericWriterCreator(DataWriterFactory factory)
    {
        _factory = factory;
    }

    public DataWriter? CreateWriter(DomainParticipant participant, string topic)
    {
        if (_dataWriter != null)
            return _dataWriter as DataWriter;

        _participant = participant;
        RegisterType(topic);
        _publisher = CreatePublisher(participant);
        _dataWriter = CreateAndWrapDataWriter(topic);
        return _dataWriter as DataWriter;
    }

    private IGenericDataWriter CreateAndWrapDataWriter(string topic)
    {
        Console.WriteLine($"Create DataWriter for topic: {topic}");
        var writer = _publisher?.CreateDataWriter(_topicInstance);
        if (writer == null)
        {
            throw new Exception("Could not create the data writer");
        }

        return _factory(writer);
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
            var result = _dataWriter.Write(data);
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