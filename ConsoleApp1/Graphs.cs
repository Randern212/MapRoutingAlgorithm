using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    class Vertex
    {
        public float positionX;
        public float positionY;
        public Dictionary<int, Edge> edges;
    }
    class Edge
    {
        public float length;
        public float speed;
        public float time => length/speed;
    }
    class Graph
    { 
        Dictionary<int, Vertex> vertices;
        public void findPath(int rootID,float rootTime, Queue<int> visitedVertices)
        {
            visitedVertices.Enqueue(rootID);

            float[] timeFromRoot = new float[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                timeFromRoot[i] = float.MaxValue;
            }
            timeFromRoot[rootID] = rootTime;
            
            int nextRootID = -1;
            float nextRootTime=float.MaxValue;
            
            foreach(var edge in vertices[rootID].edges)
            {
                timeFromRoot[edge.Key] = edge.Value.time + rootTime;
                if (nextRootTime > timeFromRoot[edge.Key])
                {
                    nextRootID = edge.Key;
                    nextRootTime = timeFromRoot[edge.Key];
                }
            }

            findPath(nextRootID, nextRootTime, visitedVertices);
        }

    }

    class GraphConstructor
    {

    }

}
