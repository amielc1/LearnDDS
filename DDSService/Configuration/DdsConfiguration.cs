using DDSService.Interface;

namespace DDSService.Configuration;

public class DdsConfiguration
{
    public int DomainId { get; set; } = 42;
    public string DCPSConfigFile { get; set; } = "-DCPSConfigFile";
    public string rtps { get; set; } = "rtps.ini";
    public IQosConfigService? QosConfig { get; set; }
}