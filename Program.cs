using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 1)
        {
            // Console.WriteLine("Variables are missing, please use the following pattern for usage: controller_program.exe <control pattern>");
            Console.Error.WriteLine("Variables are missing, please use the following pattern for usage: controller_program.exe <control pattern>[\"sequence\",\"type\"]");
            
            return;
        }

        // If you want to adjust default values in sensors change true/false to apply changes 
        bool isThereSensorsToBeAdjusted = true;

        List<Sensor> sensors = InitializeSensors(isThereSensorsToBeAdjusted);

        string controlPattern = args[0];

        if (controlPattern == "sequence")
        {
            await RunSequentialControlPattern(sensors);
        }
        else if (controlPattern == "type")
        {
            await RunTypeControlPattern(sensors);
        }
        else
        {
            Console.WriteLine("Invalid control pattern.");
        }
    }

    static List<Sensor> InitializeSensors(bool isThereSensorsToBeAdjusted)
    {
        List<Sensor> sensors = new List<Sensor>();

        // Initialize 20 sensors in alternating types
        for (int i = 1; i <= 20; i++)
        {   
            string type = "";

            if ( i % 3 == 1 )
            {
                type = "x-axis";
            } 
            else if (i % 3 == 2)
            {
                type = "y-axis";
            }
            else {
                type = "z-axis";
            }

            //string type = i % 3 == 1 ? "x-axis" : (i % 3 == 2 ? "y-axis" : "z-axis");
            
            int initialValue = 0;
            // int initialValue = i > 15 ? 3 : 0; // Offset tilted sensors
            
            sensors.Add(new Sensor(i, type, initialValue));
        }

        if (isThereSensorsToBeAdjusted)
        {
            sensors = AdjustTiltedSensors(sensors);
        }

        return sensors;
    }

    // This one is a bit generic as we are defining the cretiria for which sensors need to have different values
    static List<Sensor> AdjustTiltedSensors(List<Sensor> sensors)
    {
        int sensor_offset = 3;
         // Get the x-axis sensors from the sensor list
        var x_sensors = sensors.Where(sn => sn.Type == "x-axis").ToList();
        
        // Get the x-axis sensors that are tilt (last 5 in this case)
        var x_sensors_tilted = x_sensors.TakeLast(5).ToList();
        
        foreach(Sensor x_sn in x_sensors_tilted){
            // Console.WriteLine(sn.Position);
            // Console.WriteLine(sn.Type);
            // Console.WriteLine(sn.InitialValue);
            var tmp_sensor = sensors.FirstOrDefault(sn => sn.Position == x_sn.Position);
            
            if (tmp_sensor != null ) 
            {   
                tmp_sensor.InitialValue = sensor_offset; 
                tmp_sensor.SensorValue = sensor_offset;
            }

        }

        return sensors;
    }

    
    static async Task RunSequentialControlPattern(List<Sensor> sensors)
    {
        foreach (var sensor in sensors)
        {
            await sensor.StartRecordingAsync();
            sensor.StopRecording();
        }
    }

    static async Task RunTypeControlPattern(List<Sensor> sensors)
    {
        var tasks = new List<Task>();

        foreach (var sensor in sensors)
        {
            if (sensor.Type == "x-axis")
            {
                tasks.Add(sensor.StartRecordingAsync());
            }
        }

        await Task.WhenAll(tasks);

        foreach (var sensor in sensors)
        {
            if (sensor.Type == "x-axis")
            {
                sensor.StopRecording();
            }
        }
    }
}
