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

        public float[] sourceX;
        public float[] sourceY;
        public float[] destinationX;
        public float[] destinationY;
        public float[] R;
        public static int numOfQueries;

        public Queury() { }
        private void readQ(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Query file not found: {filePath}");
                }

                var queryTuples = QueryReader.ReadQueries(filePath);
                numOfQueries = queryTuples.Count;

                // Initialize arrays
                sourceX = new float[numOfQueries];
                sourceY = new float[numOfQueries];
                destinationX = new float[numOfQueries];
                destinationY = new float[numOfQueries];
                R = new float[numOfQueries];

                // Validate we have enough queries
                if (queryTuples.Count != numOfQueries)
                {
                    throw new InvalidDataException(
                        $"Header claims {numOfQueries} queries but found {queryTuples.Count}");
                }

                // Load data
                for (int i = 0; i < numOfQueries; i++)
                {
                    var query = queryTuples[i];
                    sourceX[i] = query.sourceX;
                    sourceY[i] = query.sourceY;
                    destinationX[i] = query.destinationX;
                    destinationY[i] = query.destinationY;
                    R[i] = query.R;

                    // Optional: Validate values
                    if (R[i] <= 0)
                    {
                        throw new InvalidDataException(
                            $"Invalid R value {R[i]} at query {i + 1}. Must be positive.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error reading queries: {ex.Message}");
                throw; // Re-throw if you want calling code to handle it
            }
        }


        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))); 
    public (Queue<int>, int) measureTrip(Graph graph, int queryIndex)
{
    Dictionary<int, Queue<int>> visitedOrders = new();
    Dictionary<int, float[]> possibleRoots = new();
    Dictionary<int, float> possibleDestinations = new();

    foreach (int id in graph.vertices.Keys)
    {
        float walkToRootDistance = euclideanDistance(
            sourceX[queryIndex], sourceY[queryIndex],
            graph.vertices[id].positionX, graph.vertices[id].positionY
        );

        if (walkToRootDistance <= R[queryIndex])
        {
            var (times, visited) = graph.FindPath(id, walkToRootDistance / 5);
            possibleRoots[id] = times;
            visitedOrders[id] = visited;
        }

        float walkToDestinationDistance = euclideanDistance(
            destinationX[queryIndex], destinationY[queryIndex],
            graph.vertices[id].positionX, graph.vertices[id].positionY
        );

        if (walkToDestinationDistance <= R[queryIndex])
        {
            possibleDestinations[id] = walkToDestinationDistance / 5;
        }
    }

    if (possibleRoots.Count == 0 || possibleDestinations.Count == 0)
        return (new Queue<int>(), -1);

    int bestDestination = possibleDestinations.OrderBy(kv => kv.Value).First().Key;
    float minTime = float.MaxValue;
    int bestRoot = -1;

    foreach (var root in possibleRoots)
    {
        if (bestDestination < root.Value.Length && root.Value[bestDestination] < minTime)
        {
            minTime = root.Value[bestDestination];
            bestRoot = root.Key;
        }
    }

    if (bestRoot == -1)
        return (new Queue<int>(), -1);

    return (visitedOrders[bestRoot], bestDestination);
}

    public static void mainn(string filePath)
    {
        Graph g = new Graph();
            //Query query = new Query();
            Queury query = new Queury();
            query.readQ(filePath);
        for (int i = 0; i < numOfQueries; i++)
        {
                //call function output here and take return value of measureTrip(g,i)
                //as printOutput(query.measureTrip(g, i);
                query.measureTrip(g, i);
        }
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
