using System;
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

    }
}
