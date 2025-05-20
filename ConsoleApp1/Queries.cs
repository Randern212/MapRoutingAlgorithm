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
                    R[i] = query.R;

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


        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))); 
        public (Queue<int>,float) measureTrip(Graph graph, int i)
        {
            Dictionary<int, float> possibleRoots = new Dictionary<int, float>();
            Dictionary<int,float> possibleDestinations  = new Dictionary<int,float>(); 
            foreach(int id in graph.vertices.Keys) 
            {
                float walkToRootDistance = euclideanDistance(sourceX[i], sourceY[i], graph.vertices[id].positionX, graph.vertices[id].positionY);

                if (walkToRootDistance <= R[i])
                {
                    possibleRoots[id]=walkToRootDistance/5;
                }
                float walkToDestinationDistance = euclideanDistance(destinationX[i], destinationY[i], graph.vertices[id].positionX, graph.vertices[id].positionY);
                if ( walkToDestinationDistance <= R[i])
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
                (tempTime,orderTemp) = graph.FindPath(id,possibleRoots[id],possibleDestinations);
                if (tempTime<minTime)
                {
                    visitedOrder = orderTemp;
                    minTime = tempTime;
                }
            }
            return (visitedOrder,minTime);
        }

        public List<(Queue<int> path, float time)> mainn(string queryFilePath, string mapFilePath)
        {
            Graph g = new Graph();
            g.readDataGraph(mapFilePath);

            Query query = new Query();
            query.readQuery(queryFilePath);

            List<(Queue<int>, float)> results = new List<(Queue<int>, float)>();

            for (int i = 0; i < Query.numOfQueries; i++)
            {
                var result = query.measureTrip(g, i);
                results.Add(result);
            }

            return results;
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


