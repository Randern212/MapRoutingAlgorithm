using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
  public class Vertex
    {
        public float positionX;
        public float positionY;
        public Dictionary<int, Edge> edges;
    }
   public class Edge
    {
        public float length;
        public float speed;
        public float time => length/speed;
    }
   public class Graph
    { 

        public (float, Queue<int>) FindPath(int rootID, float initialTime, Dictionary<int, float> possibleDestenations, Dictionary<int, Vertex> vertices)
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


        //private int edgeCount=-1;
        //private void setEdgeCount(int eC)
        //{
        //    edgeCount = eC;
        //}
        //public int getEdgeCount()
        //{
        //    return edgeCount;
        //}
       
        
   


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
