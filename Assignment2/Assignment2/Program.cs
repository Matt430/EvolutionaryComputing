using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            for(int i = 0; i < graphText.Length; ++i)
            {
                string[] graphComponents = RemoveEmpty(graphText[i].Split());
                int[] connections = new int[int.Parse(graphComponents[2])];
                for(int j = 0; j < connections.Length; ++j)
                {
                    connections[j] = int.Parse(graphComponents[j + 3]) - 1;
                }
                graph[i] = connections;
                
            }
            
            Console.WriteLine("Start Testing...");

            // Clear results
            string path = Directory.GetCurrentDirectory() + "/Results/";
            if(Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            // Run the test
            //Test to determine the best value for the number of mutations
            //ILSMutationTest(graph);

            //Test to determine the best population size for the GA
            EvolutionaryPopulationTest(graph);


            //MultiStartLocalSearch msl = new MultiStartLocalSearch(graph.Length, new GraphBipartition(graph), 2500);
            //msl.Run();

            //IteratedLocalSearch ils = new IteratedLocalSearch(graph.Length, new GraphBipartition(graph), 20, 2500);
            //ils.Run();

            //GeneticAlgorithm ga = new GeneticAlgorithm(30, graph.Length, new GraphBipartition(graph), new Crossover(), 2500);
            //ga.Run();



            //Tester.RunTests(populationCounts, stringLength, k, 0f, GeneticAlgorithm.FitnessType.Uniform);

            Console.WriteLine("Testing Finished!");
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
        }


        static string[] RemoveEmpty(string[] input)
        {
            List<string> nonEmpty = new List<string>();
            foreach(string component in input)
                if(component != "")
                    nonEmpty.Add(component);
            return nonEmpty.ToArray();
        }

        static void ILSMutationTest(int[][] graph)
        {
            int[] parameter = new int[15];
            int[,] results = new int[15, 5];
            int[,] time = new int[15, 5];
            int n = 0;

            for (int i = 2; i <= 30; i += 2)
            {
                for (int j = 0; j < 5; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();
                    IteratedLocalSearch ils = new IteratedLocalSearch(graph.Length, new GraphBipartition(graph), i, 1000);
                    ils.Run();
                    stopwatch.Stop();

                    parameter[n] = i;
                    results[n,j] = ils.bestValue;
                    time[n,j] = (int)stopwatch.ElapsedMilliseconds;    
                }

                n++;
            }
            

            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_ILS_Parameters.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Parameter;Results;Time;Results;Time;Results;Time;Results;Time;Results;Time";
            

            // Write the values into the table


            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                for (int i = 0; i < parameter.Length; i++)
                {
                    string line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", parameter[i], results[i, 0], time[i, 0], results[i, 1], time[i, 1], results[i, 2], time[i, 2], results[i, 3], time[i, 3], results[i, 4], time[i, 4]);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }

        static void EvolutionaryPopulationTest(int[][] graph)
        {
            int[] parameter = new int[5];
            int[,] results = new int[5,5];
            int[,] time = new int[5,5];
            int n = 0;

            for (int i = 50; i <= 250; i += 50)
            {
                for (int j = 0; j < 5; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();
                    GeneticAlgorithm ga = new GeneticAlgorithm(i, graph.Length, new GraphBipartition(graph), new Crossover(), 2500);
                    ga.Run();
                    stopwatch.Stop();

                    parameter[n] = i;
                    results[n,j] = ga.Result;
                    time[n,j] = (int)stopwatch.ElapsedMilliseconds;
                }
                n++;
            }


            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_Evolutionary_Populations.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Population Size;Results;Time;Results;Time;Results;Time;Results;Time;Results;Time";


            // Write the values into the table


            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                for (int i = 0; i < parameter.Length; i++)
                {
                    string line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", parameter[i], results[i, 0], time[i, 0], results[i, 1], time[i, 1], results[i, 2], time[i, 2], results[i, 3], time[i, 3], results[i, 4], time[i, 4]);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }
    }
}
