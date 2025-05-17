using Graphs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{

    class Queury
    {
        float sourceX;
        float sourceY;
        float destinationX;
        float destinationY;
        float R;
        public Queury() { }
        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))) ; 
        public void measureTrip(Graph graph)
        {
            float minTime = float.MaxValue;
            Dictionary <int,Queue<int>> visitedOrders= new Dictionary<int,Queue<int>>();
            Dictionary<int, float[]> possibleRoots = new Dictionary<int, float[]>();
            Queue<int> possibleDestinations  = new Queue<int>();

            for (int id = 0; id < graph.vertices.Count; id++)
            {
                if (euclideanDistance(sourceX, sourceY, graph.vertices[id].positionX, graph.vertices[id].positionY)<R)
                {
                    possibleRoots[id] = graph.FindPath(id, visitedOrders[id]);
                }
                if (euclideanDistance(destinationX, destinationY, graph.vertices[id].positionX, graph.vertices[id].positionY) < R)
                {
                    possibleDestinations.Enqueue(id);
                }
            }
        }
    }
    class QueryConstructor
    {
        public static Queury construct()
        {
            Queury queury = new Queury();

            return queury;
        }
    }
}
