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
        private RSSI_Module rssi;

        private int ticks = 0;

        private bool activePerfectPosition, activeGPS, activeIMU, activeRSSI;
        private double gpsLossInTicks = 0;

        private Position calculatedTrilateratePosition = null;

        public PositionProtocolLogic(double imuGyroAccuracy, double imuAccelAccuracy, double gpsLoss)
        {
            activePerfectPosition = false;
            activeGPS = true;
            activeIMU = false;
            activeRSSI = false;

            setGPSLoss(gpsLoss);

            gps = new GPS_Module();
            imu = new IMU_Module(imuGyroAccuracy, imuAccelAccuracy);
            rssi = new RSSI_Module();
        }

        public void setGPSLoss(double gpsLoss)
        {
            this.gpsLossInTicks = gpsLoss * SimulatorController.ticksPerSecond;
        }

        public bool isGPSActive()
        {
            return activeGPS;
        }

        public void assignNodes(List<SimulatedObject> nodes)
        {
            rssi.assignNodes(nodes);
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
            else if ( (activeRSSI) && (rssi.receiveMeasurementRequest(ticks)) && (calculatedTrilateratePosition!= null) ) 
            {
                actualPosition.x += expectedXTickDistance;
                actualPosition.y += expectedYTickDistance;
                actualPosition.z += expectedZTickDistance;
                
                expectedPosition.Clone(calculatedTrilateratePosition);
                
                // Reset the IMU for new theta
                imu.reset();
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

            if ((activeRSSI) && (rssi.transmitMeasurementRequest(ticks)))
            {
                // We need to request a measurement
                // Nothing really to do at the moment but right here's where we could remember "when" the request was made 
            }
            else if ((activeRSSI) && (rssi.remoteTakeMeasurement(ticks)))
            {
                // We need to request a measurement        
                calculatedTrilateratePosition = rssi.trilaterate(actualPosition);
            }

        }

        private bool shouldMeasure()
        {
            return activePerfectPosition || activeIMU || 
                ((activeGPS) && (gps.shouldMeasure(ticks))) ||
                ((activeRSSI) && (rssi.receiveMeasurementRequest(ticks)))
                ;
        }

        public void incTicks(Position actualPosition)
        {
            ++ticks;
            //if ( (gpsLossInTicks>0) && (ticks > gpsLossInTicks) &&(!activeIMU))
            //{
            //    activeGPS = false;
            //    activePerfectPosition = false;
            //    activeIMU = true;
            //}
            if ((gpsLossInTicks > 0) && (ticks > gpsLossInTicks) && (!activeRSSI))
            {
                activeGPS = false;
                activePerfectPosition = false;
                activeRSSI = true;
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
