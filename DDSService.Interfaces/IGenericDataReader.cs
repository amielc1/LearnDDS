using OpenDDSharp.DDS;

namespace DDSService.Interface
{
    public interface IGenericDataReader<T>
    {
        ReturnCode Take(List<T> dataValues, List<SampleInfo> sampleInfos);
    }
}
