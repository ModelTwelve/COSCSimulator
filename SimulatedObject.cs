using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class SimulatedObject
    {
        Random random;
        
        public double actualXPosition { get; private set; }
        public double actualYPosition { get; private set; }
        public double actualZPosition { get; private set; }

        public double expectedXPosition { get; private set; }
        public double expectedYPosition { get; private set; }
        public double expectedZPosition { get; private set; }

        public double targetXPosition { get; private set; }
        public double targetYPosition { get; private set; }
        public double targetZPosition { get; private set; }

        public double prevActualXPosition { get; private set; }
        public double prevActualYPosition { get; private set; }
        public double prevActualZPosition { get; private set; }

        public double prevExpectedXPosition { get; private set; }
        public double prevExpectedYPosition { get; private set; }
        public double prevExpectedZPosition { get; private set; }

        public double speed { get; private set; }

        public int ticks = 0;

        private double ticksPerSecond = 1000, tickSpeed;

        public SimulatedObject(int randomSeed, double startX, double startY, double startZ, double goalX, double goalY, double goalZ, double velocity)
        {
            random = new Random(randomSeed);
            actualXPosition = startX;
            actualYPosition = startY;
            actualZPosition = startZ;

            expectedXPosition = startX;
            expectedYPosition = startY;
            expectedZPosition = startZ;

            targetXPosition = goalX;
            targetYPosition = goalY;
            targetZPosition = goalZ;

            // I want to 
            speed = velocity;
            tickSpeed = speed / ticksPerSecond;
        }

        public int GPS()
        {
            // 5 meters = 0 to 16 ft would be approx +/- 16 ft from your actual
            return random.Next(-16, 16);
        }

        public bool areWeThereYet()
        {
            return actualXPosition == targetXPosition && actualYPosition == targetYPosition && actualZPosition == targetZPosition;
        }

        public double distanceFromTarget()
        {
            double tX = targetXPosition - actualXPosition;
            double tY = targetYPosition - actualYPosition;
            double tZ = targetZPosition - actualZPosition;
            double tLength = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
            return tLength;
        }

        public void stepTowards()
        {
            ++ticks;

            prevActualXPosition = actualXPosition;
            prevActualYPosition = actualYPosition;
            prevActualZPosition = actualZPosition;

            prevExpectedXPosition = expectedXPosition;
            prevExpectedYPosition = expectedYPosition;
            prevExpectedZPosition = expectedZPosition;

            // We are actually here
            double actualXDistance = targetXPosition - actualXPosition;
            double actualYDistance = targetYPosition - actualYPosition;
            double actualZDistance = targetZPosition - actualZPosition;

            // We think we're here
            double expectedXDistance = targetXPosition - expectedXPosition;
            double expectedYDistance = targetYPosition - expectedYPosition;
            double expectedZDistance = targetZPosition - expectedZPosition;

            // We actually need to move this far
            double tLength = Math.Sqrt(actualXDistance * actualXDistance + actualYDistance * actualYDistance + actualZDistance * actualZDistance);
            // We think we need to move this far
            double eLength = Math.Sqrt(expectedXDistance * expectedXDistance + expectedYDistance * expectedYDistance + expectedZDistance * expectedZDistance);

            double expectedXTickDistance = (tickSpeed * expectedXDistance / eLength);
            double expectedYTickDistance = (tickSpeed * expectedYDistance / eLength);
            double expectedZTickDistance = (tickSpeed * expectedZDistance / eLength);

            if (tLength > tickSpeed)
            {
                // move towards the goal
                //actualXPosition = actualXPosition + (speed * tX / tLength);
                //actualYPosition = actualYPosition + (speed * tY / tLength);
                //actualZPosition = actualZPosition + (speed * tZ / tLength);

                // Move towards the goal using where we think we are
                // and allowing that to affect the actual positioning
                actualXPosition = actualXPosition + expectedXTickDistance;
                actualYPosition = actualYPosition + expectedYTickDistance;
                actualZPosition = actualZPosition + expectedZTickDistance;

                // Pretend like the GPS updates every second
                if (ticks % Convert.ToInt32(ticksPerSecond) == 0)
                {
                    expectedXPosition = actualXPosition + GPS();
                    expectedYPosition = actualYPosition + GPS();
                    expectedZPosition = actualZPosition + GPS();
                }
                else
                {
                    // Else continue on your wrong path
                    expectedXPosition = expectedXPosition + expectedXTickDistance;
                    expectedYPosition = expectedYPosition + expectedYTickDistance;
                    expectedZPosition = expectedZPosition + expectedZTickDistance;
                }
            }
            else
            {
                // Last step
                if (tLength > tickSpeed)
                {
                    actualXPosition = actualXPosition + expectedXTickDistance;
                    actualYPosition = actualYPosition + expectedYTickDistance;
                    actualZPosition = actualZPosition + expectedZTickDistance;

                    expectedXPosition = actualXPosition;
                    expectedYPosition = actualYPosition;
                    expectedZPosition = actualZPosition;
                }
                else
                {
                    actualXPosition = targetXPosition;
                    actualYPosition = targetYPosition;
                    actualZPosition = targetZPosition;
                }
            }
        }
    }
}
