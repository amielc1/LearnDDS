using DDSService.Model;

namespace DDSService.Interface;

public class IQosConfigService
{
    public WriterQosConfig WriterQosConfig { get; }
    public ReaderQosConfig ReaderQosConfig { get; }

    //DomainParticipantQos
    //SubscriberQos
    //PublisherQos
    //TopicQos
}
