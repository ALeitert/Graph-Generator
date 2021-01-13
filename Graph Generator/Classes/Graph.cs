
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
    }
}
