using Graphs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{

    class Query
    {
        public float[] sourceX;
        public float[] sourceY;
        public float[] destinationX;
        public float[] destinationY;
        public float[] R;
        public static int numOfQueries;

        public Query() { }
        public void readQuery(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Query file not found: {filePath}");
                }

                var queryTuples = QueryReader.ReadQueries(filePath);
                numOfQueries = queryTuples.Count;

                sourceX = new float[numOfQueries];
                sourceY = new float[numOfQueries];
                destinationX = new float[numOfQueries];
                destinationY = new float[numOfQueries];
                R = new float[numOfQueries];

                if (queryTuples.Count != numOfQueries)
                {
                    throw new InvalidDataException(
                        $"Header claims {numOfQueries} queries but found {queryTuples.Count}");
                }

                for (int i = 0; i < numOfQueries; i++)
                {
                    var query = queryTuples[i];
                    sourceX[i] = query.sourceX;
                    sourceY[i] = query.sourceY;
                    destinationX[i] = query.destinationX;
                    destinationY[i] = query.destinationY;
                    R[i] = query.R/1000;

                    if (R[i] <= 0)
                    {
                        throw new InvalidDataException(
                            $"Invalid R value {R[i]} at query {i + 1}. Must be positive.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading queries: {ex.Message}");
                throw;
            }
        }

        public Queue<int> globalVisitedOrder = new Queue<int>();

        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2)));
        public (Queue<int>, float) measureTrip(Dictionary<int, Vertex> vertices, int i)
        {
            Graph graph = new Graph();
            Dictionary<int, float> possibleRoots = new Dictionary<int, float>();
            Dictionary<int, float> possibleDestinations = new Dictionary<int, float>();
            foreach (int id in vertices.Keys)
            {
                float walkToRootDistance = euclideanDistance(sourceX[i], sourceY[i], vertices[id].positionX, vertices[id].positionY);

                if (walkToRootDistance <= R[i])
                {
                    possibleRoots[id] = walkToRootDistance / 5;
                }
                float walkToDestinationDistance = euclideanDistance(destinationX[i], destinationY[i], vertices[id].positionX, vertices[id].positionY);
                if (walkToDestinationDistance <= R[i])
                {
                    possibleDestinations[id] = walkToDestinationDistance / 5;
                }
            }

            Queue<int> visitedOrder = new Queue<int>();
            float minTime = float.MaxValue;
            foreach (int id in possibleRoots.Keys)
            {
                float tempTime;
                Queue<int> orderTemp = new Queue<int>();
                (tempTime, orderTemp) = graph.FindPath(id, possibleRoots[id], possibleDestinations,vertices);
                if (tempTime < minTime)
                {
                    visitedOrder = orderTemp;
                    minTime = tempTime;
                }
            }
            globalVisitedOrder = visitedOrder;
            return (visitedOrder, minTime);
        }

        //float walkedToRootX= -1, walkedToRootY = -1,
        //    walkedToDistenationX = -1, walkedToDistenationY = -1;
        public float calculateWalkedDistance(Dictionary<int, Vertex> vertices, int i)
        {
            if (globalVisitedOrder == null || globalVisitedOrder.Count < 2)
                return 0; // Return 0 instead of -1 for invalid paths

            int[] path = globalVisitedOrder.ToArray();
            int firstNode = path[0];
            int lastNode = path[path.Length - 1];

            // Calculate walking distance from source to first node
            float walkToRoot = euclideanDistance(sourceX[i], sourceY[i],
                                               vertices[firstNode].positionX,
                                               vertices[firstNode].positionY);

            // Calculate walking distance from last node to destination
            float walkToDest = euclideanDistance(vertices[lastNode].positionX,
                                                vertices[lastNode].positionY,
                                                destinationX[i], destinationY[i]);

            return walkToRoot + walkToDest;
        }

        public float calcVeicleMovDistance(Dictionary<int, Vertex> vertices)
        {
            if (globalVisitedOrder == null || globalVisitedOrder.Count < 2)
                return 0;

            float totalDistance = 0;
            int[] path = globalVisitedOrder.ToArray();

            for (int i = 0; i < path.Length - 1; i++)
            {
                int from = path[i];
                int to = path[i + 1];

                if (vertices.TryGetValue(from, out var fromVertex) &&
                    fromVertex.edges.TryGetValue(to, out var edge))
                {
                    totalDistance += edge.length;
                }
            }

            return totalDistance;
        }

        public (float walkedDist, float veicledist, float allDist) calcAllMovDistance(Graph graph, int i, Dictionary<int, Vertex> vertices)
        {
            float walkedDist = -1, veicledist = -1, allDist = -1;
            walkedDist = calculateWalkedDistance(vertices, i);
            veicledist = calcVeicleMovDistance(vertices);
            allDist = walkedDist + veicledist;
            return (walkedDist,veicledist,allDist);
        }
        public List<(Queue<int> Path, float Time, float WalkedDist, float VeicledDist, float AllDist)>
        
            
            mainn(string queryFilePath, string mapFilePath, Dictionary<int, Vertex> vertices,int edgesCount
            , List<(float sourceX, float sourceY, float destinationX, float destinationY, float R, int numQueries)> queries)
        {
            Graph g = new Graph();
            //g.readDataGraph(mapFilePath);

            Query query = new Query();
            query.readQuery(queryFilePath);

            var allResults = new List<(Queue<int>, float, float, float, float)>();

            for (int i = 0; i < Query.numOfQueries; i++)
            {
                // Get path and time
                var (path, time) = query.measureTrip(vertices, i);

                // Get all distances
                var (walkedDist, veicledDist, allDist) = query.calcAllMovDistance(g, i,vertices);

                // Add all information as a single tuple
                allResults.Add((path, time, walkedDist, veicledDist, allDist));
            }

            return allResults;
        }

    }
    class QueryConstructor
    {
        public static Query construct()
        {
            Query queury = new Query();

            return queury;
        }
    }





 
}


