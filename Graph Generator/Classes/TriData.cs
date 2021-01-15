using System;
using System.Collections.Generic;

namespace GraphGenerator
{
    /// <summary>
    /// Data structure that manages the data used for triangulations.
    /// </summary>
    class TriData
    {
        Dictionary<int, int[]> triList = new Dictionary<int, int[]>();
        Dictionary<int, int[]> triNeig = new Dictionary<int, int[]>();

        int keyCtr = 0;

        public TriData(Vector[] points)
        {
            this.Points = points;
        }

        public int[][] this[int key]
        {
            get
            {
                return new int[][] { triList[key], triNeig[key] };
            }
        }

        public ICollection<int> Keys
        {
            get
            {
                return triList.Keys;
            }
        }

        public Vector[] Points { get; private set; }

        public int Add(int[] ptIDs, int[] neiKeys)
        {
            int newKey = keyCtr;
            keyCtr++;

            triList.Add(newKey, ptIDs);
            triNeig.Add(newKey, neiKeys);

            return newKey;
        }

        public bool ContainsKey(int key)
        {
            return triList.ContainsKey(key);
        }

        public void Remove(int key)
        {
            triList.Remove(key);
            triNeig.Remove(key);
        }
    }
}
