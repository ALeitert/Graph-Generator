using System;
using System.Collections.Generic;

namespace GraphGenerator
{
    class Graph
    {
        int[][] adjList = null;

        public Graph(int size)
        {
            adjList = new int[size][];
        }

        public int Size
        {
            get
            {
                return adjList.Length;
            }
        }

        public int[] this[int vId]
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

            HashSet<int>[] edges = new HashSet<int>[size];
            for (int i = 0; i < size; i++)
            {
                edges[i] = new HashSet<int>();
            }


            // --- Generate spanning tree. ---
            
            for (int vId = 1 /* 0 is root and has no parent */; vId < size; vId++)
            {
                int pId = rng.Next(vId);

                edges[vId].Add(pId);
                edges[pId].Add(vId);
            }

            
            // --- Add remaining edges. ---

            for (int i = size; i <= eCount; i++)
            {
                int uId = rng.Next(size);

                // Ensure that v is not equal to u and all other vertices are equally likely.
                int vId = rng.Next(size - 1);
                if (vId >= uId) vId++;

                edges[uId].Add(vId);
                edges[vId].Add(uId);
            }


            // --- Build adjacency list. ---

            for (int vId = 0; vId < size; vId++)
            {
                g.adjList[vId] = new int[edges[vId].Count];
                edges[vId].CopyTo(g[vId]);
            }


            return g;
        }
    }
}
