using System;
using System.Drawing;
using System.Collections.Generic;

namespace GraphGenerator
{
    class Geometry
    {
        public static Vector[] GetRandomPoints(int size, double width, double height)
        {
            Random rng = new Random();
            Vector[] points = new Vector[size];

            for (int i = 0; i < size; i++)
            {
                points[i] = new Vector
                (
                    rng.NextDouble() * width,
                    rng.NextDouble() * height
                );
            }

            return points;
        }

        public static Graph Triangulate(Vector[] points)
        {
            int size = points.Length;
            Graph g = new Graph(size);

            // ToDo: Implement.

            return g;
        }

        /// <summary>
        /// Returns a rectangle that sorounds the given points.
        /// </summary>
        public static RectangleF GetSurroundingRectangle(Vector[] points)
        {
            if (points == null || points.Length == 0)
            {
                return new RectangleF();
            }


            // --- Determine sorounding rectangle. ---

            float minX = float.MaxValue;
            float minY = float.MaxValue;

            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < points.Length; i++)
            {
                Vector vecV = points[i];

                minX = Math.Min(minX, (float)vecV.X);
                minY = Math.Min(minY, (float)vecV.Y);

                maxX = Math.Max(maxX, (float)vecV.X);
                maxY = Math.Max(maxY, (float)vecV.Y);
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Returns an equilateral triangle that sorounds the given points.
        /// </summary>
        public static Vector[] GetSurroundingTriangle(Vector[] points)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }


            RectangleF rect = GetSurroundingRectangle(points);


            // --- Compute triangle sorounding the rectangle. ---

            /*
             * Let A, B, C, and D be the points of the rectangle placed as follows:
             * 
             *   D C
             *   A B
             * 
             * Let X, Y, and Z be the points of the triangle placed as follows:
             * 
             *    Z
             *   X Y
             * 
             * We then have XA = AD / tan 60.
             */

            double ad = rect.Height;
            double xa = ad / Math.Tan(Math.PI / 3);
            double xy = 2.0 * ad + rect.Width;

            // Height of triangle is 0.5 * sqrt(3) * len.
            double triH = 0.5 * Math.Sqrt(3) * xy;

            Vector X = new Vector(rect.X - xa, rect.Y);
            Vector Y = new Vector(X.X + xy, rect.Y);
            Vector Z = new Vector(X.X + 0.5 * xy, rect.Y + triH);

            return new Vector[] { X, Y, Z };
        }

        /// <summary>
        /// Allows to determine if a point is left or right a line xy.
        /// </summary>
        /// <returns>
        /// A positive number if p is right of y from perspective of x in a left-handed coordinate system.
        /// </returns>
        public static double GetScalar(Vector x, Vector y, Vector p)
        {
            return (x.X - y.X) * (p.Y - y.Y) - (x.Y - y.Y) * (p.X - y.X);
        }

        /// <summary>
        /// Determines if a given point is within the given triangle.
        /// </summary>
        public static bool InTriangle(Vector[] triangle, Vector p)
        {
            if (triangle == null || triangle.Length != 3)
            {
                return false;
            }

            double scalXY = GetScalar(triangle[0], triangle[1], p);
            double scalYZ = GetScalar(triangle[1], triangle[2], p);
            double scalZX = GetScalar(triangle[2], triangle[0], p);

            return
                (scalXY <= 0 && scalYZ <= 0 && scalZX <= 0) ||
                (scalXY >= 0 && scalYZ >= 0 && scalZX >= 0);
        }
    }
}
