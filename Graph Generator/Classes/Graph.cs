using System;
using System.Collections.Generic;

namespace GraphGenerator
{
    class Graph
    {
        HashSet<int>[] adjList = null;

        public Graph(int size)
        {
            adjList = new HashSet<int>[size];
            for (int vId = 0; vId < size; vId++)
            {
                adjList[vId] = new HashSet<int>();
            }
        }

        public int Size
        {
            get
            {
                return adjList.Length;
            }
        }

        public HashSet<int> this[int vId]
        {
            get
            {
                return adjList[vId];
            }
        }

        /// <summary>
        /// Generates a random connected graph with the given number of vertices and edges.
        /// </summary>
        public static Graph NewRandom(int size, int eCount, int seed)
        {
            Random rng = new Random(seed);
            Graph g = new Graph(size);


            // --- Generate spanning tree. ---

            for (int vId = 1 /* 0 is root and has no parent */; vId < size; vId++)
            {
                int pId = rng.Next(vId);

                g[vId].Add(pId);
                g[pId].Add(vId);
            }


            // --- Add remaining edges. ---

            for (int i = size; i <= eCount; i++)
            {
                int uId = rng.Next(size);

                // Ensure that v is not equal to u and all other vertices are equally likely.
                int vId = rng.Next(size - 1);
                if (vId >= uId) vId++;

                g[uId].Add(vId);
                g[vId].Add(uId);
            }


            return g;
        }


        /// <summary>
        /// "Draws" the graph with a force-based approach and returns the computed coordinates.
        /// </summary>
        public Vector[] Draw()
        {
            return Draw(null);
        }

        /// <summary>
        /// "Draws" the graph with a force-based approach and returns the computed coordinates.
        /// </summary>
        public Vector[] Draw(Vector[] points)
        {
            if (points == null || points.Length != Size)
            {
                // Vertices will be placed randomly in a square of length 2 sqrt(n).
                double sqrLen = 2.0 * Math.Sqrt(Size);
                points = Geometry.GetRandomPoints(Size, sqrLen, sqrLen);
            }


            // --- Draw the graph. ---

            Vector[] forces = new Vector[Size];

            // Parameters for drawing.
            // No idea which does what but they seem to work well.

            double d0 = 7;
            double lf = 5;
            double kg = 0.5 * d0 * d0;
            double kf = 0.5 / (d0 - lf);

            const double forceSpeed = 0.05;
            const int iterations = 100;


            for (int i = 0; i < iterations; i++)
            {
                // Compute forces on each vertex.
                for (int vId = 0; vId < Size; vId++)
                {
                    Vector vecV = points[vId];
                    forces[vId] = new Vector(0, 0);

                    for (int uId = 0; uId < Size; uId++)
                    {
                        if (uId == vId) continue;

                        Vector vecU = points[uId];

                        Vector vecUV = vecV - vecU;
                        Vector dirUV = vecUV.Normalize();

                        double disUV = vecUV.Length;

                        double g = kg / Math.Pow(disUV, 2);

                        if (adjList[vId].Contains(uId))
                        {
                            double f = kf * (disUV - lf);
                            forces[vId] += dirUV * (g - f);
                        }
                        else
                        {
                            forces[vId] += vecUV * g;
                        }
                    }
                }

                // Apply forces.
                for (int vId = 0; vId < Size; vId++)
                {
                    Vector vFor = forces[vId];
                    double dis = vFor.Length;
                    points[vId] += forceSpeed * dis * vFor.Normalize();
                }
            }


            return points;
        }
    }
}
