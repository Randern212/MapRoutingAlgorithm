using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
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
        public Dictionary<int, Vertex> vertices;

        public (float, Queue<int>) FindPath(int rootID, float initialTime, Dictionary<int, float> possibleDestenations)
        {
            int[] appendedVertices = new int[vertices.Count];
            float[] timeFromRoot = new float[vertices.Count];
            Array.Fill(timeFromRoot, float.MaxValue);
            timeFromRoot[rootID] = initialTime;
            int bestDestination = -1;
            Queue<int> visitedOrder = new Queue<int>();
            Array.Fill(appendedVertices, -1);

            float minTime = float.MaxValue;

            var priorityQueue = new PriorityQueue<int, float>();
            priorityQueue.Enqueue(rootID, 0);

            while (priorityQueue.Count > 0)
            {
                int currentID = priorityQueue.Dequeue();
                if (timeFromRoot[currentID] == float.MaxValue)
                    break;


                foreach (var edge in vertices[currentID].edges)
                {
                    int neighborID = edge.Key;
                    float newTime = timeFromRoot[currentID] + edge.Value.time;

                    if (newTime < timeFromRoot[neighborID])
                    {
                        if (possibleDestenations.ContainsKey(neighborID))
                        {
                            newTime = newTime + possibleDestenations[neighborID];
                            if (newTime < minTime)
                            {
                                minTime = newTime;
                                bestDestination = neighborID;
                            }
                        }

                        timeFromRoot[neighborID] = newTime;
                        appendedVertices[neighborID] = (currentID);
                        priorityQueue.Enqueue(neighborID, newTime);
                    }
                }
            }
            if (bestDestination != -1)
            {
                var path = new Stack<int>();
                for (int at = bestDestination; at != -1; at = appendedVertices[at])
                {
                    path.Push(at);
                }

                while (path.Count > 0)
                {
                    visitedOrder.Enqueue(path.Pop());
                }
            }

            return (minTime, visitedOrder);
        }


        private int edgeCount=-1;
        private void setEdgeCount(int eC)
        {
            edgeCount = eC;
        }
        public int getEdgeCount()
        {
            return edgeCount;
        }
        public void readDataGraph(string filePath)
        {
            vertices = new Dictionary<int, Vertex>();

            var lines = File.ReadAllLines(filePath);
            int vertexCount = int.Parse(lines[0]);

            for (int i = 1; i <= vertexCount; i++)
            {
                var parts = lines[i].Split(' ');
                int id = int.Parse(parts[0]);
                float x = float.Parse(parts[1]);
                float y = float.Parse(parts[2]);

                vertices[id] = new Vertex
                {
                    positionX = x,
                    positionY = y,
                    edges = new Dictionary<int, Edge>()
                };
            }

            int edgeCount = int.Parse(lines[vertexCount + 1]);
            setEdgeCount(edgeCount);
            for (int i = vertexCount + 2; i < vertexCount + 2 + edgeCount; i++)
            {
                var parts = lines[i].Split(' ');
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                float length = float.Parse(parts[2]);
                float speed = float.Parse(parts[3]);

                vertices[from].edges[to] = new Edge { length = length, speed = speed };
                vertices[to].edges[from] = new Edge { length = length, speed = speed }; 
            }
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
