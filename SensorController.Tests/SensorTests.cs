using System;
using System.Threading.Tasks;
using Xunit;

namespace SensorController.Tests
{
    public class SensorTests
    {
        [Fact]
        public async Task StartRecordingAsync_StartsRecording()
        {
            var sensor = new Sensor(1, "x-axis", 0);
            await sensor.StartRecordingAsync();
            Assert.True(sensor.IsRecording);
        }

        [Fact]
        public void StopRecording_StopsRecording()
        {
            var sensor = new Sensor(1, "x-axis", 0);
            sensor.StartRecordingAsync();
            sensor.StopRecording();
            Assert.False(sensor.IsRecording);
        }

        [Fact]
        public void SetAdjustedSensorInitialValue_SetsValue()
        {
            var sensor = new Sensor(1, "x-axis", 0);
            sensor.SetAdjustedInitialSensorValue(10);
            Assert.Equal(10, sensor.InitialValue);
        }

        [Fact]
        public void SetSensorValue_SetsValue()
        {
            var sensor = new Sensor(1, "x-axis", 0);
            sensor.SetSensorValue(10);
            Assert.Equal(10, sensor.SensorValue);
        }
        
    }
}
