using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class SimulatedObject
    {        
        public const int objectSize = 6;

        private PositionProtocolLogic positionLogic;

        public Position actualPosition = new Position();
        public Position prevActualPosition = new Position();
        public Position expectedPosition = new Position();
        public Position prevExpectedPosition = new Position();
        public Position targetPosition = new Position();

        public double speed { get; private set; }       

        private double tickSpeed;        

        public SimulatedObject(Position origin, Position destination, double velocity, double imuGyroAccuracy, double imuAccelAccuracy, int roundTripTime, double gpsLoss)
        {
            positionLogic = new PositionProtocolLogic(imuGyroAccuracy, imuAccelAccuracy, roundTripTime, gpsLoss);

            actualPosition.Clone(origin);
            prevActualPosition.Clone(origin);
            expectedPosition.Clone(origin);
            prevExpectedPosition.Clone(origin);
            targetPosition.Clone(destination);                 

            // I want to 
            speed = velocity;
            tickSpeed = speed / SimulatorController.ticksPerSecond;
        }

        public void assignNodes(List<SimulatedObject> nodes, double? gpsLoss=null)
        {
            positionLogic.assignNodes(nodes);
            if (gpsLoss!=null)
            {
                positionLogic.setGPSLoss(gpsLoss.Value);
            }
        }

        public bool isGPSActive()
        {
            return positionLogic.isGPSActive();
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
            double theta = Math.Atan(expectedXDistance / expectedYDistance) * 180.0 / Math.PI; // polar
            double phi = Math.Atan(
                Math.Sqrt(Position.square(expectedXDistance) + Position.square(expectedYDistance)) / 
                expectedZDistance) * 180.0 / Math.PI; // azimuthal

            theta = Math.Abs(theta);
            phi = Math.Abs(phi);
            //if (phi < 0)
            //{
            //    phi += 180;
            //}

            // We actually need to move this far
            double actualDistance = actualPosition.distanceFrom(targetPosition);
            // We think we need to move this far
            double expectedDistance = expectedPosition.distanceFrom(targetPosition);

            double expectedXTickDistance = (tickSpeed * expectedXDistance / expectedDistance);
            double expectedYTickDistance = (tickSpeed * expectedYDistance / expectedDistance);
            double expectedZTickDistance = (tickSpeed * expectedZDistance / expectedDistance);

            double radius = Position.calculateRadius(expectedXTickDistance, expectedYTickDistance, expectedZTickDistance);

            positionLogic.updatePosition(actualDistance, tickSpeed,
            targetPosition, actualPosition, expectedPosition,
            expectedXTickDistance, expectedYTickDistance, expectedZTickDistance,
            theta, phi, radius);
            
        }        
    }
}
