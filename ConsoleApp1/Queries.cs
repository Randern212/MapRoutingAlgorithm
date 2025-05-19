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
        float R; //in kilometers
        public Queury() { }
        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))); 
        public (Queue<int>,float) measureTrip(Graph graph)
        {
            Dictionary<int, float> possibleRoots = new Dictionary<int, float>();
            Dictionary<int,float> possibleDestinations  = new Dictionary<int,float>(); 
            foreach(int id in graph.vertices.Keys) 
            {
                float walkToRootDistance = euclideanDistance(sourceX, sourceY, graph.vertices[id].positionX, graph.vertices[id].positionY);

                if (walkToRootDistance<=R)
                {
                    possibleRoots[id]=walkToRootDistance/5;
                }
                float walkToDestinationDistance = euclideanDistance(destinationX, destinationY, graph.vertices[id].positionX, graph.vertices[id].positionY);
                if ( walkToDestinationDistance <= R)
                {
                    possibleDestinations[id]= walkToDestinationDistance/5;
                }
            }

            Queue<int> visitedOrder = new Queue<int>();
            float minTime=float.MaxValue;
            foreach (int id in possibleRoots.Keys)
            {
                float tempTime;
                Queue<int> orderTemp = new Queue<int>();
                (tempTime,orderTemp) = graph.FindPath(id,possibleRoots[id],possibleDestinations);\
                if (tempTime<minTime)
                {
                    visitedOrder = orderTemp;
                    minTime = tempTime;
                }
            }
            return (visitedOrder,minTime);
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
