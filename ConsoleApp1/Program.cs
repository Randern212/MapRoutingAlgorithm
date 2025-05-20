using Graphs;
using Queries;

namespace main
{
    class Program
    {
        public static void sendRecieve() {
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string mapFilePath = Path.Combine(projectDir, "ConsoleApp1","TEST CASES", "[1] Sample Cases", "Input", "map4.txt");
            string queryFilePath = Path.Combine(projectDir, "ConsoleApp1", "TEST CASES", "[1] Sample Cases", "Input", "queries4.txt");
            Graph g = new Graph();
            Query q = new Query();
            List<(Queue<int> path, float time)> finalResult;
            //g.readDataGraph(mapFilePath);
            finalResult= q.mainn(queryFilePath, mapFilePath);
            printOutput(finalResult);
        }

        public static void printOutput(List<(Queue<int> path, float time)> finalResult)
        {
            for (int i = 0; i < finalResult.Count; i++)
            {
                var (path, time) = finalResult[i];

                Console.WriteLine($"Query #{i + 1}:");
                Console.WriteLine($"Time: {time:F2} seconds");

                if (path.Count == 0)
                {
                    Console.WriteLine("Path: No valid path found within the walking range.");
                }
                else
                {
                    Console.WriteLine("Path: " + string.Join(" -> ", path));
                }

                Console.WriteLine(new string('-', 40)); 
            }
        }

         static void Main(string[] args)
        {
            sendRecieve();
        }
    }
}