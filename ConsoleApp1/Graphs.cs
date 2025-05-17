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

        public float FindPath(int rootID, Queue<int> visitedOrder)
        {
            float[] timeFromRoot = new float[vertices.Count];
            Array.Fill(timeFromRoot, float.MaxValue);
            timeFromRoot[rootID] = 0;
            float totalTime = 0;

            var priorityQueue = new PriorityQueue<int, float>();
            priorityQueue.Enqueue(rootID, 0);

            while (priorityQueue.Count > 0)
            {
                int currentID = priorityQueue.Dequeue();
                if (timeFromRoot[currentID] == float.MaxValue)
                    break;

                visitedOrder.Enqueue(currentID);
                totalTime += timeFromRoot[currentID];

                foreach (var edge in vertices[currentID].edges)
                {
                    int neighborID = edge.Key;
                    float newTime = timeFromRoot[currentID] + edge.Value.time;

                    if (newTime < timeFromRoot[neighborID])
                    {
                        timeFromRoot[neighborID] = newTime;
                        priorityQueue.Enqueue(neighborID, newTime);
                    }
                }
            }
            return totalTime*60; //*60 to convert it to minutes
        }
    }

    class GraphConstructor
    {
        public static Graph construct()
        {
            Graph graph = new Graph();

            return graph; 
        }
    }

}
