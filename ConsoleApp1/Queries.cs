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

<<<<<<< HEAD
    class Query
    {
=======


    class Queury
    {

>>>>>>> shehap
        public float[] sourceX;
        public float[] sourceY;
        public float[] destinationX;
        public float[] destinationY;
        public float[] R;
        public static int numOfQueries;

<<<<<<< HEAD
        public Query() { }
        public void storeData( List<(float sourceX, float sourceY, float destinationX, float destinationY, float R)> queries)
        {
           
               

                //var queryTuples = QueryReader.ReadQueries(filePath);
                numOfQueries = queries.Count; //queryTuples.Count;

                sourceX = new float[numOfQueries];
                sourceY = new float[numOfQueries];
                destinationX = new float[numOfQueries];
                destinationY = new float[numOfQueries];
                R = new float[numOfQueries];

                if (queries.Count != numOfQueries)
                {
                    throw new InvalidDataException(
                        $"Header claims {numOfQueries} queries but found {queries.Count}");
                }

                for (int i = 0; i < numOfQueries; i++)
                {
                    var query = queries[i];
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
        
            
            mainn( Dictionary<int, Vertex> vertices,int edgesCount
            , List<(float sourceX, float sourceY, float destinationX, float destinationY, float R)> queries )
        {
            Graph g = new Graph();

            Query query = new Query();
            query.storeData(queries);

            var allResults = new List<(Queue<int>, float, float, float, float)>();

            for (int i = 0; i < Query.numOfQueries; i++)
            {
                var (path, time) = query.measureTrip(vertices, i);

                var (walkedDist, veicledDist, allDist) = query.calcAllMovDistance(g, i,vertices);

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
=======
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
>>>>>>> shehap

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

<<<<<<< HEAD




 
=======
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

>>>>>>> shehap
}


