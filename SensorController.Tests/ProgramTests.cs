using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SensorController.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task RunSequentialControlPattern_SequencePatternWorks()
        {
            var sensors = new List<Sensor>
            {
                new Sensor(1, "x-axis", 0),
                new Sensor(2, "y-axis", 0),
                new Sensor(3, "z-axis", 0),
            };

            await Program.RunSequentialControlPattern(sensors);

            foreach (var sensor in sensors)
            {
                Assert.False(sensor.IsRecording);
            }
        }

        [Fact]
        public async Task RunTypeControlPattern_TypePatternWorks()
        {
            var sensors = new List<Sensor>
            {
                new Sensor(1, "x-axis", 0),
                new Sensor(2, "y-axis", 0),
                new Sensor(3, "z-axis", 0),
                new Sensor(4, "x-axis", 0),
                new Sensor(5, "y-axis", 0),
            };

            await Program.RunTypeControlPattern(sensors);

            foreach (var sensor in sensors)
            {
                if (sensor.Type == "x-axis")
                {
                    Assert.True(sensor.RecordedAtLeastForOnce);
                }
                else
                {
                    Assert.False(sensor.RecordedAtLeastForOnce);
                }
            }
        }
    }
}
