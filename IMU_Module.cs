using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class IMU_Module
    {
        private double degreesPerTickTheta = 0, degreesPerTickPhi = 0;
        private double posnegTheta = 1, posnegPhi = 1;
        private int ticksUntilMeasure = Convert.ToInt32(SimulatorController.ticksPerSecond); // For now just use this       
        private double imuTicks = 0;

        private double? theta=null, phi, radius;

        public IMU_Module(double degreesPerHourTheta, double degreesPerHourPhi)
        {
            // degreesPerHour divide once more by 3600 for degress per sec and then again by the 
            // number of simulation ticks per second and we should be good to go!
            degreesPerTickTheta = (degreesPerHourTheta / 3600.0) / SimulatorController.ticksPerSecond;
            degreesPerTickPhi = (degreesPerHourPhi / 3600.0) / SimulatorController.ticksPerSecond;
        }

        private double getAngle(double number)
        {
            return Math.PI * number / 180.0;
        }

        public void reset()
        {
            this.theta = null;
        }

        public void Calculate(Position actualPosition, Position expectedPosition, 
            double expectedXTickDistance, double expectedYTickDistance, double expectedZTickDistance,
            double theta, double phi, double radius)
        {
            if (this.theta==null)
            {
                this.theta = theta;
                this.phi = phi;
                this.radius = radius;
                if (SimRandom.GetRandom(-1,1)>0)
                {
                    posnegTheta *= -1;
                }
                if (SimRandom.GetRandom(-1, 1) > 0)
                {
                    posnegPhi *= -1;
                }
            }
            double x, y, z;
            
            // Now apply new measrue
            ++imuTicks;
            this.theta += (degreesPerTickTheta * posnegTheta);
            this.phi += (degreesPerTickPhi * posnegPhi);

            y = this.radius.Value * Math.Sin(getAngle(this.phi.Value)) * Math.Cos(getAngle(this.theta.Value));
            x = this.radius.Value * Math.Sin(getAngle(this.phi.Value)) * Math.Sin(getAngle(this.theta.Value));
            z = this.radius.Value * Math.Cos(getAngle(this.phi.Value));

            // Continue on your wrong path
            expectedPosition.x += expectedXTickDistance;
            expectedPosition.y += expectedYTickDistance;
            expectedPosition.z += expectedZTickDistance;

            actualPosition.x += x;
            actualPosition.y += y;
            actualPosition.z += z;
        }
    }
}
