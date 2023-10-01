using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Sensor
{
    public int Position { get; }
    public string Type { get; }
    public int InitialValue { get; set; }
    public int SensorValue { get; set; }
    public bool IsRecording { get; private set; }

    private int SensorRecordsDataDefaultValue {get; } 

    public Sensor(int position, string type, int initialValue)
    {
        Position = position;
        Type = type;
        InitialValue = initialValue;
        SensorValue = initialValue;
        IsRecording = false;

        SensorRecordsDataDefaultValue = 5;
    }

    public async Task StartRecordingAsync()
    {
        if (!IsRecording)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} Sensor {Position} {Type} start {SensorValue}");

            IsRecording = true;

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

    public void SetSensorValue(int value)
    {
        SensorValue = value;
    }
}
