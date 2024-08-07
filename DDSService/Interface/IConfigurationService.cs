using DDSService.Model;

namespace DDSService.Interface;

public class IConfigurationService
{
    WriterQosConfig WriterQosConfig { get; }
    ReaderQosConfig ReaderQosConfig { get; }
}
