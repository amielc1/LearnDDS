using OpenDDSharp.DDS;

namespace DDSService.Model;

public class ReaderQosConfig
    {
        public ReliabilityQosPolicyKind ReliabilityKind { get; set; }
        public OwnershipQosPolicyKind OwnershipKind { get; set; }
        public DurabilityQosPolicyKind DurabilityKind { get; set; }
        public HistoryQosPolicyKind HistoryKind { get; set; }
        public int HistoryDepth { get; set; }
        public int MinimumSeparation { get; set; }
   
        public ReaderQosConfig()
        {
            ReliabilityKind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            OwnershipKind = OwnershipQosPolicyKind.SharedOwnershipQos;
            DurabilityKind = DurabilityQosPolicyKind.VolatileDurabilityQos;
            HistoryKind = HistoryQosPolicyKind.KeepLastHistoryQos;
            HistoryDepth = 1;
            MinimumSeparation = 0;
        } 
    }
