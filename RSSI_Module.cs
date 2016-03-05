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

        private List<SimulatedObject> nodes = new List<SimulatedObject>();
        public RSSI_Module()
        {

        }

        public bool shouldMeasure(int ticks)
        {
            // Poll every second
            return nodes.Count>=3 && ticks % SimulatorController.ticksPerSecond == 0;
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
            double r1 = Position.distanceFrom(actualP1, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);
            double r2 = Position.distanceFrom(actualP2, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);
            double r3 = Position.distanceFrom(actualP3, actualP0) + SimRandom.GetRandom(accuracyMin, accuracyMax);

            var rv = Position.trilaterate(expectedP1, expectedP2, expectedP3, r1, r2, r3);

            return rv;
        }
    }

    
}
