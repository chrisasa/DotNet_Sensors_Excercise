using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;


namespace SensorController
{
    public class Program
    {

        static readonly int NumberOfSensors = 20;

        // If you want to adjust default values in sensors change true/false to apply changes 
        static readonly bool IsThereSensorsToBeAdjusted = true;

        static readonly Hashtable SensorsTypeHTable = new Hashtable() {
        {1,"x-axis"},
        {2,"y-axis"},
        {3,"z-axis"},
        
        // {4,"AA-axis"},
        // {5,"BB-axis"},
        // {6,"CC-axis"},
        // {7,"DD-axis"},
        // {8,"EE-axis"}
    };

        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                // Console.WriteLine("Variables are missing, please use the following pattern for usage: controller_program.exe <control pattern>");
                Console.Error.WriteLine("Variables are missing, please use the following pattern" +
                                        "for usage: controller_program.exe <control pattern>[\"sequence\",\"type\"]");

                return;
            }

            List<Sensor> sensors = InitializeSensors();

            if (IsThereSensorsToBeAdjusted)
            {
                sensors = AdjustTiltedSensors(sensors);
            }

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

        static List<Sensor> InitializeSensors()
        {
            List<Sensor> sensors = new List<Sensor>();

            int numberOfSensorTypes = SensorsTypeHTable.Count;

            // Initialize 20 sensors in alternating types
            for (int i = 1; i <= NumberOfSensors; i++)
            {
                int initialValue = 0;
                string type = "";

                // Version 1, the number of sensor types is predefined
                // if ( i % 3 == 1 )
                // {
                //     type = "x-axis";
                // } 
                // else if (i % 3 == 2)
                // {
                //     type = "y-axis";
                // }
                // else {
                //     type = "z-axis";
                // }

                // Version 2, the number of sensor types is unknown
                var getKeyForSensorTypeHashTable = i % numberOfSensorTypes;

                // Check if mod 0, then get the last value in Hashtable
                getKeyForSensorTypeHashTable = getKeyForSensorTypeHashTable == 0 ? numberOfSensorTypes : getKeyForSensorTypeHashTable;

                type = SensorsTypeHTable[getKeyForSensorTypeHashTable].ToString() ?? "";

                sensors.Add(new Sensor(i, type, initialValue));
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

            // Find and replace the values in the sensors list
            foreach (Sensor x_sn in x_sensors_tilted)
            {
                // We are treating Position value as unique Id and we are matching entries based on it
                var tmp_sensor = sensors.FirstOrDefault(sn => sn.Position == x_sn.Position);

                // Null check or enclose in try-catch. In this case may not benecessary as we are getting 
                // the values from the same list so for sure they should exist
                if (tmp_sensor != null)
                {
                    tmp_sensor.SetAdjustedInitialSensorValue(sensor_offset);
                    tmp_sensor.SetSensorValue(sensor_offset);
                }

            }

            return sensors;
        }

        public static async Task RunSequentialControlPattern(List<Sensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                await sensor.StartRecordingAsync();
                sensor.StopRecording();
            }
        }

        public static async Task RunTypeControlPattern(List<Sensor> sensors)
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
}