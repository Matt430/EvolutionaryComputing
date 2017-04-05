using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Assign how large the population should be.
            //int[] populationCounts = new int[] { 50, 100, 250, 500 };

            // Create a nested array structure to represent the graph
            //string[] graphText = File.ReadLines(Directory.GetCurrentDirectory() + "/Graphs/Graph10.txt").ToArray();
            string[] graphText = File.ReadLines(Directory.GetCurrentDirectory() + "/Graphs/Graph500.txt").ToArray();
            int[][] graph = new int[graphText.Length][];
            for (int i = 0; i < graphText.Length; ++i)
            {
                string[] graphComponents = RemoveEmpty(graphText[i].Split());
                int[] connections = new int[int.Parse(graphComponents[2])];
                for (int j = 0; j < connections.Length; ++j)
                {
                    connections[j] = int.Parse(graphComponents[j + 3]) - 1;
                }
                graph[i] = connections;

            }

            Console.WriteLine("Start Testing...");

            // Clear results
            string path = Directory.GetCurrentDirectory() + "/Results/";
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            // Run the test
            Tester.RunTests(graph);

            Console.WriteLine("Testing Finished!");
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
        }


        static string[] RemoveEmpty(string[] input)
        {
            List<string> nonEmpty = new List<string>();
            foreach (string component in input)
                if (component != "")
                    nonEmpty.Add(component);
            return nonEmpty.ToArray();
        }
    }
}
