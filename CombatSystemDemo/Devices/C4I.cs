using System;
using DDSService;
using DDSService.Adapters;
using DDSService.Configuration;
using DDSService.Interface;
using DDSService.MessageBroker.DDS;
using MessageBroker.Core.Interfaces;
using MissionModule;
using OpenDDSharp.DDS;

namespace CombatSystemDemo.Devices
{
    public class C4I
    {
        private static int counter = 0;
        private readonly IDataReaderCreator _creator;
        private readonly IDdsService _ddsService;
        private readonly DdsConfiguration _config;
        private readonly ISubscriber _subscriber;


        public C4I()
        {

            IGenericDataReader<Mission> Factory(DataReader reader) => new MissionDataReaderAdapter(reader);
            var _ReaderCreator = new GenericReaderCreator<MissionTypeSupportAdapter, Mission>(Factory);




            DataWriterFactory.Register<Mission>(writer => new MissionDataWriterAdapter(writer));  

            _creator = _ReaderCreator;
            _config = new DdsConfiguration();
            _config.Topic = "MissionTopic";
            _ddsService = new OpenDdsService(_config);
            _subscriber = new DdsSubscriber(_ddsService, _creator);
        }

        public void Import()
        {
            _subscriber.Subscribe(_config.Topic, OnMessageArrived);
        }

        private void OnMessageArrived(object sender, object e)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}  {((Mission)e).Name}");
        }
    }
}
