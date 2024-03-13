using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MissionModule;
using MissionSubscriber.Interface;
using OpenDDSharp.DDS;

namespace MissionSubscriber
{
    public class MissionReaderCreator : IDataReaderCreator
    {
        public event EventHandler<object> DataReceived = delegate { };
        private Subscriber _subscriber;
        private DomainParticipant _participant;
        private CancellationTokenSource _cancellationTokenSource = new();

        public DataReader CreateDataReader(DomainParticipant participant, string topic)
        {
            try
            {
                _participant = participant;
                RegisterType(participant, topic);
                CreateSubscriber(participant);
                var dataReader = CreateAndWrapDataReader(topic);

                // Start listening for events in a background task
                Task.Run(() => WaitForEvents(dataReader, _cancellationTokenSource.Token));
                return dataReader;
            }
            catch (Exception ex)
            {
                LogError($"Error creating data reader: {ex.Message}");
                throw;
            }
        }

        private void RegisterType(DomainParticipant participant, string topic)
        {
            var missionTypeSupport = new MissionTypeSupport();
            var missionTypeName = missionTypeSupport.GetTypeName();
            var result = missionTypeSupport.RegisterType(participant, missionTypeName);

            Log($"Register participant {participant.DomainId}, with MissionType {missionTypeName}");
            if (result != ReturnCode.Ok)
            {
                throw new Exception($"Could not register type: {result}");
            }

            var topicInstance = participant.CreateTopic(topic, missionTypeName);
            Log($"Create Topic {topic} with participant {participant.DomainId} and MissionType {missionTypeName}");
            if (topicInstance == null)
            {
                throw new Exception("Could not create the message topic");
            }
        }

        private void CreateSubscriber(DomainParticipant participant)
        {
            _subscriber = participant.CreateSubscriber();
            Log($"Create Subscriber on participant {participant.DomainId}");
            if (_subscriber == null)
            {
                throw new Exception("Could not create the subscriber");
            }
        }

        private MissionDataReader CreateAndWrapDataReader(string topic)
        {
            var dataReader = _subscriber.CreateDataReader(_participant.LookupTopicDescription(topic));
            Log($"Create DataReader on participant {_participant.DomainId}");
            if (dataReader == null)
            {
                throw new Exception("Could not create the DataReader");
            }

            var missionDataReader = new MissionDataReader(dataReader);
            Log("Wrap DataReader with MissionDataReader helper class");
            return missionDataReader;
        }

        public void UnSubscribe()
        {
            _cancellationTokenSource.Cancel();
            _participant.DeleteSubscriber(_subscriber);
        }

        private async Task WaitForEvents(MissionDataReader missionDataReader, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ProcessDataEvents(missionDataReader);
                await Task.Delay(100);
            }
        }

        private void ProcessDataEvents(MissionDataReader missionDataReader)
        {
            var mask = missionDataReader.StatusChanges;
            if ((mask & StatusKind.DataAvailableStatus) == 0) return;

            var receivedData = new List<Mission>();
            var receivedInfo = new List<SampleInfo>();
            var result = missionDataReader.Take(receivedData, receivedInfo);

            if (result == ReturnCode.Ok)
            {
                foreach (var info in receivedInfo)
                {
                    if (!info.ValidData) continue;
                    var index = receivedInfo.IndexOf(info);
                    var mission = receivedData[index];
                    DataReceived?.Invoke(this, mission);
                }
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void LogError(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
