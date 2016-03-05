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

        public static double distanceFrom(Position p1, Position p2)
        {
            double xDistance = p2.x - p1.x;
            double yDistance = p2.y - p1.y;
            double zDistance = p2.z - p1.z;
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

        public static double calculateRadius(Position p)
        {
            return Math.Sqrt(square(p.x) + square(p.y) + square(p.z));
        }

        public static double calculateRadius(double a, double b, double c)
        {
            return Math.Sqrt(square(a)+square(b)+square(c));
        }

        // Credits to: https://github.com/gheja
        // based on: https://github.com/gheja/trilateration.js/blob/master/trilateration.js
        // which was based on: https://en.wikipedia.org/wiki/Trilateration

        private static double dot(Position p1, Position p2)
        {
            return p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
        }

        private static Position vector_subtract(Position p1, Position p2)
        {
            Position rv = new Position();

            rv.x = p1.x - p2.x;
            rv.y = p1.y - p2.y;
            rv.z = p1.z - p2.z;

            return rv;
        }

        private static Position vector_add(Position p1, Position p2)
        {
            Position rv = new Position();

            rv.x = p1.x + p2.x;
            rv.y = p1.y + p2.y;
            rv.z = p1.z + p2.z;

            return rv;
        }

        private static Position vector_divide(Position p1, double number)
        {
            Position rv = new Position();

            rv.x = p1.x / number;
            rv.y = p1.y / number;
            rv.z = p1.z / number;

            return rv;
        }

        private static Position vector_multiply(Position p1, double number)
        {
            Position rv = new Position();

            rv.x = p1.x * number;
            rv.y = p1.y * number;
            rv.z = p1.z * number;

            return rv;
        }

        private static Position vector_cross(Position p1, Position p2)
        {
            Position rv = new Position();

            rv.x = p1.y * p2.z - p1.z * p2.y;
            rv.y = p1.z * p2.x - p1.x * p2.z;
            rv.z = p1.x * p2.y - p1.y * p2.x;

            return rv;
        }

        public static Position trilaterate(Position p1, Position p2, Position p3, double r1, double r2, double r3)
        {
            var ex = vector_divide(vector_subtract(p2, p1), calculateRadius(vector_subtract(p2, p1)));

            var i = dot(ex, vector_subtract(p3, p1));
            var a = vector_subtract(vector_subtract(p3, p1), vector_multiply(ex, i));
            var ey = vector_divide(a, calculateRadius(a));
            var ez = vector_cross(ex, ey);
            var d = calculateRadius(vector_subtract(p2, p1));
            var j = dot(ey, vector_subtract(p3, p1));

            var x = (square(r1) - square(r2) + square(d)) / (2 * d);
            var y = (square(r1) - square(r3) + square(i) + square(j)) / (2 * j) - (i / j) * x;
            var z = Math.Sqrt(square(r1) - square(x) - square(y));

            Position rv = vector_add(p1, vector_add(vector_multiply(ex, x), vector_multiply(ey, y)));

            //var p4a = vector_add(a, vector_multiply(ez, z));
            //var p4b = vector_subtract(a, vector_multiply(ez, z));

            return rv;
        }
    }
}
