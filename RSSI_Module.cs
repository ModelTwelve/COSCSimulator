using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class RSSI_Module
    {
        private const double accuracyMin = -3.0;
        private const double accuracyMax = 3.0;

        private int rttTicks = 0;
        private int halfRTTticks = 0;
        private int nextRequest = 0;
        private int nextReceive = 0;

        private List<SimulatedObject> nodes = new List<SimulatedObject>();
        public RSSI_Module(int roundTripTime)
        {
            // roundTripTime is in millisec
            rttTicks = roundTripTime * Convert.ToInt32(SimulatorController.ticksPerSecond / 1000.0);
            halfRTTticks = rttTicks / 2;
        }

        public bool remoteMeasurementTransmitRequest(int ticks)
        {
            // Request a measure every second
            bool rv = nodes.Count >= 3 && ticks % SimulatorController.ticksPerSecond == 0;
            if ( (rv)&&(nextRequest==0)&&(nextReceive==0) ) {
                // Currently only allowing one outstanding request
                nextRequest = ticks + halfRTTticks;
                nextReceive = ticks + rttTicks;
            }
            return rv;
        }

        public bool remoteMeasurementRequestReceived(int ticks)
        {
            // Remote receives request 1/2 RTT from transmist
            return nodes.Count >= 3 && ticks == nextRequest;
        }

        public bool remoteMeasurementRequestReturned(int ticks)
        {
            // Receive measurement RTT from transmist
            bool rv = nodes.Count >= 3 && ticks == nextReceive;
            if (rv) {
                // Reset packet counters
                nextRequest = 0;
                nextReceive = 0;
            }
            return rv;
        }

        public void assignNodes(List<SimulatedObject> nodes)
        {
            this.nodes = nodes;
        }

        public Position trilaterate(Position actualP0)
        {
            Position expectedP1 = nodes[0].expectedPosition;
            Position expectedP2 = nodes[1].expectedPosition;
            Position expectedP3 = nodes[2].expectedPosition;

            Position actualP1 = nodes[0].actualPosition;
            Position actualP2 = nodes[1].actualPosition;
            Position actualP3 = nodes[2].actualPosition;

            // RSSI should be able to get distance measurement to within 3 feet
            // For this we use the actual distance
            double r1 = Position.distanceFrom(actualP1, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);
            double r2 = Position.distanceFrom(actualP2, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);
            double r3 = Position.distanceFrom(actualP3, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);

            // Now we trilaterate using the expected positions but use the RSSI distance/radius calculations from above.
            var rv = Position.trilaterate(expectedP1, expectedP2, expectedP3, r1, r2, r3);

            return rv;
        }
    }

    
}
