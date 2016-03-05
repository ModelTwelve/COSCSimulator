using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class Position
    {
        public double x = 0, y = 0, z = 0;
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
            return calculateRadius(xDistance, yDistance, zDistance);
        }

        public void Clone(Position p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }

        public static double square(double number)
        {
            return Math.Pow(number, 2);
        }

        public static double calculateRadius(double a, double b, double c)
        {
            return Math.Sqrt(square(a)+square(b)+square(c));
        }
    }
}
