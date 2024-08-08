using DDSService.Interface;
using DDSService.Model;

namespace DDSService;

public class QosConfigService : IQosConfigService
{
    public WriterQosConfig WriterQosConfig { get; }
    public ReaderQosConfig ReaderQosConfig { get; }
    public QosConfigService()
    {
        WriterQosConfig = new WriterQosConfig();
        ReaderQosConfig = new ReaderQosConfig();
    }
}
