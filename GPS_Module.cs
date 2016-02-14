using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class GPS_Module
    {
        const int minValue = -16; // Ft
        const int maxValue = 16; // Ft        

        Random random;
        public GPS_Module(Random random)
        {
            this.random = random;
        }

        public bool shouldMeasure(int ticks)
        {
            // Poll every second
            return ticks % SimulatorController.ticksPerSecond == 0;
        }

        public void Calculate(Position expectedPosition, Position actualPosition)
        {
            // 5 meters = 0 to 16 ft would be approx +/- 16 ft from your actual
            expectedPosition.x = actualPosition.x + random.Next(minValue, maxValue);
            expectedPosition.y = actualPosition.y + random.Next(minValue, maxValue);
            expectedPosition.z = actualPosition.z + random.Next(minValue, maxValue);
        }
    }
}
