using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SensorController
{
    public class Sensor
    {
        public int Position { get; }
        public string Type { get; }
        public int InitialValue { get; private set; }
        public int SensorValue { get; private set; }
        public bool IsRecording { get; private set; }
        public bool RecordedAtLeastForOnce { get; private set; }

        private int SensorRecordsDataDefaultValue { get; }

        public Sensor(int position, string type, int initialValue)
        {
            Position = position;
            Type = type;
            InitialValue = initialValue;
            SensorValue = initialValue;
            IsRecording = false;
            RecordedAtLeastForOnce = false;

            SensorRecordsDataDefaultValue = 5;
        }

        public async Task StartRecordingAsync()
        {
            if (!IsRecording)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} Sensor {Position} {Type} start {SensorValue}");

                IsRecording = true;
                
                RecordedAtLeastForOnce = true;

                this.SensorValue = SensorRecordsDataDefaultValue - InitialValue;

                await Task.Delay(1000); // Simulate recording for 1 second
            }
        }

        public void StopRecording()
        {
            if (IsRecording)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} Sensor {Position} {Type} stop {SensorValue}");

                IsRecording = false;
            }
        }

        public void SetAdjustedInitialSensorValue(int value)
        {
            InitialValue = value;
        }

        public void SetSensorValue(int value)
        {
            SensorValue = value;
        }
    }
}