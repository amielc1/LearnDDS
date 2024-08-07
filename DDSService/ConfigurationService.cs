using DDSService.Interface;
using DDSService.Model;

namespace DDSService;

public class ConfigurationService : IConfigurationService
{
    public WriterQosConfig WriterQosConfig { get; }
    public ReaderQosConfig ReaderQosConfig { get; }
    public ConfigurationService()
    {
        WriterQosConfig = new WriterQosConfig();
        ReaderQosConfig = new ReaderQosConfig();
    }
}
