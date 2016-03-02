using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class PositionProtocolLogic
    {
        private GPS_Module gps;
        private IMU_Module imu;

        private int ticks = 0;

        private bool activePerfectPosition, activeGPS, activeIMU;
        private double gpsLossInTicks = 0;

        public PositionProtocolLogic(Random random, double imu_Theta, double imu_Phi, double gpsLoss)
        {
            activePerfectPosition = false;
            activeGPS = true;
            activeIMU = false;
            this.gpsLossInTicks = gpsLoss*1000;
            gps = new GPS_Module();
            imu = new IMU_Module(imu_Theta, imu_Phi);
        }

        private void getExpectedPosition(Position actualPosition, Position expectedPosition,
            double expectedXTickDistance, double expectedYTickDistance, double expectedZTickDistance,
            double theta, double phi, double radius)
        {
            if (activePerfectPosition)
            {
                actualPosition.x += expectedXTickDistance;
                actualPosition.y += expectedYTickDistance;
                actualPosition.z += expectedZTickDistance;

                expectedPosition.Clone(actualPosition);
            }
            else if ((activeGPS) && (gps.shouldMeasure(ticks)))
            {
                actualPosition.x += expectedXTickDistance;
                actualPosition.y += expectedYTickDistance;
                actualPosition.z += expectedZTickDistance;

                gps.Calculate(expectedPosition, actualPosition);
            }
            else if (activeIMU)
            {
                // Constantly apply
                imu.Calculate(actualPosition, expectedPosition, expectedXTickDistance, expectedYTickDistance, expectedZTickDistance,
                    theta, phi, radius);
            }
            else
            {
                throw new Exception("ERROR: No other position measuring devices exist!");
            }
            
        }

        private bool shouldMeasure()
        {
            return activePerfectPosition || activeIMU ||
                ((activeGPS) && (gps.shouldMeasure(ticks)));
        }

        public void incTicks(Position actualPosition)
        {
            ++ticks;
            if ( (ticks > gpsLossInTicks) &&(!activeIMU))
            {
                activeGPS = false;
                activePerfectPosition = false;
                activeIMU = true;
            }
        }

        public void updatePosition(double actualDistance, double tickSpeed,
            Position targetPosition, Position actualPosition, Position expectedPosition,
            double expectedXTickDistance, double expectedYTickDistance, double expectedZTickDistance,
            double theta, double phi, double radius)
        {
            if (actualDistance > tickSpeed)
            {
                // Move towards the goal using where we think we are
                // and allowing that to affect the actual positioning                

                if (shouldMeasure())
                {
                    getExpectedPosition(actualPosition, expectedPosition,
                        expectedXTickDistance, expectedYTickDistance, expectedZTickDistance,
                        theta, phi, radius);
                }
                else
                {
                    // Else continue on your wrong path
                    actualPosition.x += expectedXTickDistance;
                    actualPosition.y += expectedYTickDistance;
                    actualPosition.z += expectedZTickDistance;

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
    }
}
