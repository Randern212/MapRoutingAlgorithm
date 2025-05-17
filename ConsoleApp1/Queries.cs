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
        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))); 
        public (Queue<int>,int) measureTrip(Graph graph)
        {
            Dictionary <int,Queue<int>> visitedOrders= new Dictionary<int,Queue<int>>();
            Dictionary<int, float[]> possibleRoots = new Dictionary<int, float[]>();
            Dictionary<int,float> possibleDestinations  = new Dictionary<int,float>(); 
            foreach(int id in graph.vertices.Keys) 
            {
                float walkToRootDistance = euclideanDistance(sourceX, sourceY, graph.vertices[id].positionX, graph.vertices[id].positionY);

                if (walkToRootDistance<=R)
                {
                    (possibleRoots[id],visitedOrders[id]) = graph.FindPath(id,walkToRootDistance/5);
                }
                float walkToDestinationDistance = euclideanDistance(destinationX, destinationY, graph.vertices[id].positionX, graph.vertices[id].positionY);
                if ( walkToDestinationDistance <= R)
                {
                    possibleDestinations[id]= walkToDestinationDistance/5;
                }
            }

            if (possibleRoots.Count == 0 || possibleDestinations.Count == 0)
                return (new Queue<int>(), -1);
            float minTime= float.MaxValue;
            int bestDestination= possibleDestinations.Min().Key;
            int bestRoot = -1;
            foreach(var root in possibleRoots)
            {
                if (minTime > root.Value[bestDestination])
                {
                    bestRoot = root.Key;
                }
            }
            return (visitedOrders[bestRoot],bestDestination);
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
