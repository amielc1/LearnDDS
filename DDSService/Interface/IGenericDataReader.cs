using OpenDDSharp.DDS;
using System.Xml.Linq;

namespace DDSService.Interface
{
    public interface IGenericDataReader<T>
    {

        ReturnCode Take(List<T> dataValues, List<SampleInfo> sampleInfos,EventHandler<T> DataReceived);
    }
}
