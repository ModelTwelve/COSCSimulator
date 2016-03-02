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

        public SimulatedObject(double startX, double startY, double startZ, double goalX, double goalY, double goalZ, double velocity, double imu_Theta, double imu_Phi, double gpsLoss)
        {
            positionLogic = new PositionProtocolLogic(random, imu_Theta, imu_Phi, gpsLoss);

            actualPosition = new Position(startX, startY, startZ);
            prevActualPosition = new Position(startX, startY, startZ);
            expectedPosition = new Position(startX, startY, startZ);
            prevExpectedPosition = new Position(startX, startY, startZ);
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

        private double getAngle(double number)
        {
            return Math.PI * number / 180.0;
        }

        public void stepTowards()
        {
            positionLogic.incTicks(actualPosition);
           
            prevActualPosition.Clone(actualPosition);
            prevExpectedPosition.Clone(expectedPosition);            

            // We think we're here
            double expectedXDistance = targetPosition.x - expectedPosition.x;
            double expectedYDistance = targetPosition.y - expectedPosition.y;
            double expectedZDistance = targetPosition.z - expectedPosition.z;

            // Get more precise angles from full expected distance
            double theta = Math.Abs(
                Math.Atan(expectedXDistance / expectedYDistance) * 180.0 / Math.PI
                ); // polar
            double phi = Math.Abs(
                Math.Atan(
                Math.Sqrt(expectedXDistance * expectedXDistance + expectedYDistance * expectedYDistance) / 
                expectedZDistance) * 180.0 / Math.PI
                ); // azimuthal  

            // We actually need to move this far
            double actualDistance = actualPosition.distanceFrom(targetPosition);
            // We think we need to move this far
            double expectedDistance = expectedPosition.distanceFrom(targetPosition);

            double expectedXTickDistance = (tickSpeed * expectedXDistance / expectedDistance);
            double expectedYTickDistance = (tickSpeed * expectedYDistance / expectedDistance);
            double expectedZTickDistance = (tickSpeed * expectedZDistance / expectedDistance);

            double radius = Math.Sqrt(expectedXTickDistance * expectedXTickDistance + 
                expectedYTickDistance * expectedYTickDistance + 
                expectedZTickDistance * expectedZTickDistance);

            positionLogic.updatePosition(actualDistance, tickSpeed,
            targetPosition, actualPosition, expectedPosition,
            expectedXTickDistance, expectedYTickDistance, expectedZTickDistance,
            theta, phi, radius);
            
        }        
    }
}
