using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{ 
    public static class SimRandom
    {
        private static Random random = new Random();

        public static double GetRandom(double minvalue, double maxValue)
        {
            // Get total distance between min and max
            double total = Math.Abs(minvalue) + Math.Abs(maxValue);
            total *= random.NextDouble();
            return minvalue + total;
        }

        public static double[] GetRandomPointWithinSphere(double maxRadius)
        {
            double radiusSquared = maxRadius * maxRadius;
            double minValue = maxRadius * -1;

            // Having uniformly distributed points within a sphere is hard!
            // Let's cheat and do it more simply by first making a "bounding box"
            // And randomly picking a point within that box.
            // If that point has a radius > our max then pick another point.
            // This theoretically could result in an infinite loop but I'll take my chances.
            double x = GetRandom(minValue, maxRadius);
            double y = GetRandom(minValue, maxRadius);
            double z = GetRandom(minValue, maxRadius);
            while ((x * x) + (y * y) + (z * z) > radiusSquared)
            {
                x = GetRandom(minValue, maxRadius);
                y = GetRandom(minValue, maxRadius);
                z = GetRandom(minValue, maxRadius);
            }

            return new double[] { x, y, z };
        }
    }
}
