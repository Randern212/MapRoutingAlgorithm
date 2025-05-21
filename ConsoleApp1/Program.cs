using Graphs;
using Queries;
using System.Diagnostics;
namespace main
{
    class Program
    {

        public static (Dictionary<int, Vertex> vertices, int edgeCount) readDataGraph(string filePath)
        {
            Dictionary<int, Vertex> vertices = new Dictionary<int, Vertex>();

            var lines = File.ReadAllLines(filePath);
            int vertexCount = int.Parse(lines[0]);

            for (int i = 1; i <= vertexCount; i++)
            {
                var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
            for (int i = vertexCount + 2; i < vertexCount + 2 + edgeCount; i++)
            {
                var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                float length = float.Parse(parts[2]);
                float speed = float.Parse(parts[3]);

                vertices[from].edges[to] = new Edge { length = length, speed = speed };
                vertices[to].edges[from] = new Edge { length = length, speed = speed };
            }

            return (vertices, edgeCount);
        }

        public static (Dictionary<int, Vertex> graphVertices, int totalEdges, List<(float sourceX, float sourceY, float destinationX, float destinationY, float R)> queryTuples) readInputs()
        {
            // Construct file paths
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string mapFilePath = Path.Combine(projectDir, "ConsoleApp1", "TEST CASES", "[1] Sample Cases", "Input", "map5.txt");
            string queryFilePath = Path.Combine(projectDir, "ConsoleApp1", "TEST CASES", "[1] Sample Cases", "Input", "queries5.txt");

            // Read queries from file
            var queryTuples = QueryReader.ReadQueries(queryFilePath);

            // Read graph from file
            var (graphVertices, totalEdges) = Program.readDataGraph(mapFilePath);

            // Return all three items as a tuple
            return (graphVertices, totalEdges, queryTuples);
        }

        public static void printOutput(List<(Queue<int> path, float time, float walkedDist, float veicledDist, float allDist)> finalResult)
        {
            for (int i = 0; i < finalResult.Count; i++)
            {
                var (path, time, walkedDist, veicledDist, allDist) = finalResult[i];

                Console.WriteLine($"Query #{i + 1}:");

                if (path.Count == 0 || walkedDist < 0 || veicledDist < 0)
                {
                    Console.WriteLine("No valid path found within walking range");
                    Console.WriteLine(new string('-', 40));
                    continue;
                }

                Console.WriteLine(string.Join(" ", path));
                Console.WriteLine($"{time*60:F2} mins");
                Console.WriteLine($"{allDist:F2} km");
                Console.WriteLine($"{walkedDist:F2} km");
                Console.WriteLine($"{veicledDist:F2} km");
                Console.WriteLine(new string('-', 40));
            }
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //sendRecieve();
            var (graphVertices, totalEdges, queryTuples) = readInputs();

            Query q = new Query();
            //Stopwatch stopwatch2 = new Stopwatch();
            //stopwatch2.Start();
            var finalResult = q.mainn(graphVertices, totalEdges, queryTuples);
            //stopwatch2.Stop();
            //Console.WriteLine($"{stopwatch2.ElapsedMilliseconds} ms");

            printOutput(finalResult);
            stopwatch.Stop();
            Console.WriteLine(); 

            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");



        }
  
    
    }
}

public static class QueryReader
{
    public static List<(float sourceX, float sourceY, float destinationX, float destinationY, float R)> ReadQueries(string filePath)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        int numQueries = int.Parse(lines[0]);

        var queries = new List<(float, float, float, float, float)>(); // 5-element tuples

        for (int i = 1; i <= numQueries; i++)
        {
            var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5) continue;

            float sourceX = float.Parse(parts[0]);
            float sourceY = float.Parse(parts[1]);
            float destinationX = float.Parse(parts[2]);
            float destinationY = float.Parse(parts[3]);
            float R = float.Parse(parts[4]);

            queries.Add((sourceX, sourceY, destinationX, destinationY, R)); // match 5-element tuple
        }

        return queries;
    }

}
