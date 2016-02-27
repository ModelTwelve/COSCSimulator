using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class GPS_Module
    {
        public const float maxRadius = 16; // Ft        
        
        public GPS_Module()
        {
        }

        public bool shouldMeasure(int ticks)
        {
            // Poll every second
            return ticks % SimulatorController.ticksPerSecond == 0;
        }

        public void Calculate(Position expectedPosition, Position actualPosition)
        {
            // 5 meters = 0 to 16 ft would be approx +/- 16 ft from your actual

            double[] randomPoint = SimRandom.GetRandomPointWithinSphere(maxRadius);

            expectedPosition.x = actualPosition.x + randomPoint[0];
            expectedPosition.y = actualPosition.y + randomPoint[1];
            expectedPosition.z = actualPosition.z + randomPoint[2];
        }
    }
}
