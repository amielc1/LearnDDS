namespace CombatSystem.Configuration;

public class DdsConfiguration
{
    public int DomainId { get; set; } = 42;
    public string Topic { get; set; } = "MissionTopic";
    public string DCPSConfigFile { get; set; } = "-DCPSConfigFile";
    public string rtps { get; set; } = "rtps.ini";
}