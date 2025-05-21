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
        public (Queue<int>, float) measureTrip(Graph graph, int i)
        {
            Dictionary<int, float> possibleRoots = new Dictionary<int, float>();
            Dictionary<int, float> possibleDestinations = new Dictionary<int, float>();
            foreach (int id in graph.vertices.Keys)
            {
                float walkToRootDistance = euclideanDistance(sourceX[i], sourceY[i], graph.vertices[id].positionX, graph.vertices[id].positionY);

                if (walkToRootDistance <= R[i])
                {
                    possibleRoots[id] = walkToRootDistance / 5;
                }
                float walkToDestinationDistance = euclideanDistance(destinationX[i], destinationY[i], graph.vertices[id].positionX, graph.vertices[id].positionY);
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
                (tempTime, orderTemp) = graph.FindPath(id, possibleRoots[id], possibleDestinations);
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
        public float calculateWalkedDistance(Graph graph, int i)
        {
            if (globalVisitedOrder == null || globalVisitedOrder.Count < 2)
                return 0; // Return 0 instead of -1 for invalid paths

            int[] path = globalVisitedOrder.ToArray();
            int firstNode = path[0];
            int lastNode = path[path.Length - 1];

            // Calculate walking distance from source to first node
            float walkToRoot = euclideanDistance(sourceX[i], sourceY[i],
                                               graph.vertices[firstNode].positionX,
                                               graph.vertices[firstNode].positionY);

            // Calculate walking distance from last node to destination
            float walkToDest = euclideanDistance(graph.vertices[lastNode].positionX,
                                                graph.vertices[lastNode].positionY,
                                                destinationX[i], destinationY[i]);

            return walkToRoot + walkToDest;
        }

        public float calcVeicleMovDistance(Graph graph)
        {
            if (globalVisitedOrder == null || globalVisitedOrder.Count < 2)
                return 0;

            float totalDistance = 0;
            int[] path = globalVisitedOrder.ToArray();

            for (int i = 0; i < path.Length - 1; i++)
            {
                int from = path[i];
                int to = path[i + 1];

                if (graph.vertices.TryGetValue(from, out var fromVertex) &&
                    fromVertex.edges.TryGetValue(to, out var edge))
                {
                    totalDistance += edge.length;
                }
            }

            return totalDistance;
        }

        public (float walkedDist, float veicledist, float allDist) calcAllMovDistance(Graph graph, int i)
        {
            float walkedDist = -1, veicledist = -1, allDist = -1;
            walkedDist = calculateWalkedDistance(graph, i);
            veicledist = calcVeicleMovDistance(graph);
            allDist = walkedDist + veicledist;
            return (walkedDist,veicledist,allDist);
        }
        public List<(Queue<int> Path, float Time, float WalkedDist, float VeicledDist, float AllDist)>
        mainn(string queryFilePath, string mapFilePath)
        {
            Graph g = new Graph();
            g.readDataGraph(mapFilePath);

            Query query = new Query();
            query.readQuery(queryFilePath);

            var allResults = new List<(Queue<int>, float, float, float, float)>();

            for (int i = 0; i < Query.numOfQueries; i++)
            {
                // Get path and time
                var (path, time) = query.measureTrip(g, i);

                // Get all distances
                var (walkedDist, veicledDist, allDist) = query.calcAllMovDistance(g, i);

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





    public static class QueryReader
    {
        public static List<(float sourceX, float sourceY, float destinationX, float destinationY, float R, int numQueries)> ReadQueries(string filePath)
        {
            var lines = File.ReadAllLines(filePath).ToList();
            int numQueries = int.Parse(lines[0]);

            var queries = new List<(float, float, float, float, float, int)>();

            for (int i = 1; i <= numQueries; i++)
            {
                var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 5) continue;

                float sourceX = float.Parse(parts[0]);
                float sourceY = float.Parse(parts[1]);
                float destinationX = float.Parse(parts[2]);
                float destinationY = float.Parse(parts[3]);
                float R = float.Parse(parts[4]);

                queries.Add((sourceX, sourceY, destinationX, destinationY, R, numQueries));
            }

            return queries;
        }
    }

}


