using DDSService.Model;

namespace DDSService.Interface;

public class IConfigurationService
{
    public WriterQosConfig WriterQosConfig { get; }
    public ReaderQosConfig ReaderQosConfig { get; }
}
