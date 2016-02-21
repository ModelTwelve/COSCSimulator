using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class Position
    {
        public double x=0, y=0, z=0;
        public Position()
        {
        }

        public Position(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double distanceFrom(Position target)
        {
            double xDistance = target.x - this.x;
            double yDistance = target.y - this.y;
            double zDistance = target.z - this.z;
            double totalLength = Math.Sqrt(xDistance * xDistance + yDistance * yDistance + zDistance * zDistance);
            return totalLength;
        }

        public void Clone(Position p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }
    }

    public class SimulatedObject
    {        
        public const int objectSize = 4;

        private PositionProtocolLogic positionLogic;
        private Random random;

        public Position actualPosition;
        public Position prevActualPosition;
        public Position expectedPosition;
        public Position prevExpectedPosition;
        public Position targetPosition;    

        public double speed { get; private set; }       

        private double tickSpeed;

        public SimulatedObject(double startX, double startY, double startZ, double goalX, double goalY, double goalZ, double velocity)
        {
            positionLogic = new PositionProtocolLogic(random);

            actualPosition = new Position(startX, startY, startZ);
            prevActualPosition = new Position();
            expectedPosition = new Position(startX, startY, startZ);
            prevExpectedPosition = new Position();
            targetPosition = new Position(goalX, goalY, goalZ);            

            // I want to 
            speed = velocity;
            tickSpeed = speed / SimulatorController.ticksPerSecond;
        }
        
        public bool areWeThereYet()
        {
            return actualPosition.x == targetPosition.x && 
                actualPosition.y == targetPosition.y && 
                actualPosition.z == targetPosition.z;
        }

        public double distanceFromTarget()
        {
            return actualPosition.distanceFrom(targetPosition);
        }

        public void stepTowards()
        {
            positionLogic.incTicks();

            prevActualPosition.Clone(actualPosition);
            prevExpectedPosition.Clone(expectedPosition);            

            // We think we're here
            double expectedXDistance = targetPosition.x - expectedPosition.x;
            double expectedYDistance = targetPosition.y - expectedPosition.y;
            double expectedZDistance = targetPosition.z - expectedPosition.z;

            // We actually need to move this far
            double actualDistance = actualPosition.distanceFrom(targetPosition);
            // We think we need to move this far
            double expectedDistance = expectedPosition.distanceFrom(targetPosition);

            double expectedXTickDistance = (tickSpeed * expectedXDistance / expectedDistance);
            double expectedYTickDistance = (tickSpeed * expectedYDistance / expectedDistance);
            double expectedZTickDistance = (tickSpeed * expectedZDistance / expectedDistance);

            if (actualDistance > tickSpeed)
            {
                // Move towards the goal using where we think we are
                // and allowing that to affect the actual positioning
                actualPosition.x += expectedXTickDistance;
                actualPosition.y += expectedYTickDistance;
                actualPosition.z += expectedZTickDistance;

                if (positionLogic.shouldMeasure())
                {
                    positionLogic.getExpectedPosition(expectedPosition, actualPosition);
                }
                else
                {
                    // Else continue on your wrong path
                    expectedPosition.x += expectedXTickDistance;
                    expectedPosition.y += expectedYTickDistance;
                    expectedPosition.z += expectedZTickDistance;
                }
            }
            else
            {
                // Last step
                if (actualDistance > tickSpeed)
                {
                    actualPosition.x += expectedXTickDistance;
                    actualPosition.y += expectedYTickDistance;
                    actualPosition.z += expectedZTickDistance;

                    expectedPosition.Clone(actualPosition);
                }
                else
                {
                    actualPosition.Clone(targetPosition);
                }
            }
        }

        //public static int[] pickRandomXYZWithinSphere(int radius)
        //{

        //}
    }
}
