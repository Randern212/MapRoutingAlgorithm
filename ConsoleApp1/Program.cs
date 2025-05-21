using Graphs;
using Queries;
using System.Diagnostics;
namespace main
{
    class Program
    {

        public static void sendRecieve()
        {
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string mapFilePath = Path.Combine(projectDir, "ConsoleApp1", "TEST CASES", "[3] Large Cases", "Input", "SFMap.txt");
            string queryFilePath = Path.Combine(projectDir, "ConsoleApp1", "TEST CASES", "[3] Large Cases", "Input", "SFQueries.txt");
            Query q = new Query();
           
            
            
            var finalResult = q.mainn(queryFilePath, mapFilePath);
            printOutput(finalResult);
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

            sendRecieve();
            stopwatch.Stop();
            Console.WriteLine(); 

            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");



        }
    }
}