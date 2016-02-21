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

        private int ticks = 0;

        private bool activePerfectPosition, activeGPS;

        public PositionProtocolLogic(Random random)
        {
            activePerfectPosition = false;
            activeGPS = true;
            
            gps = new GPS_Module();
        }

        public void getExpectedPosition(Position expectedPosition, Position actualPosition)
        {
            if (activePerfectPosition)
            {
                expectedPosition.Clone(actualPosition);
            }
            else if ((activeGPS) && (gps.shouldMeasure(ticks)))
            {
                gps.Calculate(expectedPosition, actualPosition);
            }
            else
            {
                throw new Exception("ERROR: No other position measuring devices exist!");
            }
            
        }

        public bool shouldMeasure()
        {
            return activePerfectPosition ||
                ((activeGPS) && (gps.shouldMeasure(ticks)));
        }

        public void incTicks()
        {
            ++ticks;
        }
    }
}
