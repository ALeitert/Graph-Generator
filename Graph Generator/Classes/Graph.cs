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

    }
}
