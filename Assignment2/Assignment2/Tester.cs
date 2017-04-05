using System;
using System.Diagnostics;
using System.IO;

namespace Assignment2
{
    class Tester
    {
        // Run 25 tests with the given settings
        public static void RunTests(int[][] graph)
        {
            Console.WriteLine("Start Testing MLS...");
            MLSTest(graph);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing ILS...");
            ILSMutationTest(graph);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing GLS...");
            EvolutionaryPopulationTest(graph);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing GILS...");
            EvolutionaryIterativePopulationTest(graph);
            Console.WriteLine("Testing Finished!");
        }

        private static void MLSTest(int[][] graph)
        {
            int[] time = new int[25];
            for (int i = 0; i < 25; ++i)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                MultiStartLocalSearch mls = new MultiStartLocalSearch(graph.Length, new GraphBipartition(graph), 1000);
                mls.Run();

                stopwatch.Stop();
                time[i] = (int)stopwatch.ElapsedMilliseconds;
            }

            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_MLS.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Time";

            // Write the values into the table
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                float[] temp = new float[25];
                string line;
                for (int i = 0; i < time.Length; i++)
                {
                    line = string.Format("{0}", time[i]);
                    writer.WriteLine(line);
                    temp[i] = time[i];
                }
                line = string.Format("{0}", CalculateMean(temp));
                writer.WriteLine(line);
                line = string.Format("{0}", CalculateStandardDeviation(temp));
                writer.WriteLine(line);
                writer.Flush();
            }
        }

        private static void ILSMutationTest(int[][] graph)
        {
            int[] parameter = new int[15];
            float[] resultsMean = new float[15];
            float[] resultsSD = new float[15];
            float[] timeMean = new float[15];
            float[] timeSD = new float[15];
            int n = 0;

            for (int i = 2; i <= 30; i += 2)
            {
                float[] resultValues = new float[25];
                float[] timeValues = new float[25];
                for (int j = 0; j < 25; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    IteratedLocalSearch ils = new IteratedLocalSearch(graph.Length, new GraphBipartition(graph), i, 1000);
                    ils.Run();

                    stopwatch.Stop();

                    parameter[n] = i;
                    resultValues[j] += ils.bestValue;
                    timeValues[j] += (int)stopwatch.ElapsedMilliseconds;
                }
                resultsMean[n] = CalculateMean(resultValues);
                resultsSD[n] = CalculateStandardDeviation(resultValues);
                timeMean[n] = CalculateMean(timeValues);
                timeSD[n] = CalculateStandardDeviation(timeValues);
                n++;
            }

            // Save results
            SaveTable(Directory.GetCurrentDirectory() + "/Results/ResultTable_ILS_Parameters.csv", "Parameter, Results Mean, Results SD, Time Mean, Time SD", parameter, resultsMean, resultsSD, timeMean, timeSD);
        }

        private static void EvolutionaryPopulationTest(int[][] graph)
        {
            int[] parameter = new int[5];
            float[] resultsMean = new float[5];
            float[] resultsSD = new float[5];
            float[] timeMean = new float[5];
            float[] timeSD = new float[5];
            int n = 0;

            for (int i = 50; i <= 250; i += 50)
            {
                float[] resultValues = new float[25];
                float[] timeValues = new float[25];
                for (int j = 0; j < 25; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    GeneticAlgorithm ga = new GeneticAlgorithm(i, graph.Length, new GraphBipartition(graph), new Crossover(), 1000);
                    ga.Run();

                    stopwatch.Stop();

                    parameter[n] = i;
                    resultValues[j] += ga.Result;
                    timeValues[j] += (int)stopwatch.ElapsedMilliseconds;
                }
                resultsMean[n] = CalculateMean(resultValues);
                resultsSD[n] = CalculateStandardDeviation(resultValues);
                timeMean[n] = CalculateMean(timeValues);
                timeSD[n] = CalculateStandardDeviation(timeValues);
                n++;
            }

            // Save results
            SaveTable(Directory.GetCurrentDirectory() + "/Results/ResultTable_ILS_Parameters.csv", "Parameter, Results Mean, Results SD, Time Mean, Time SD", parameter, resultsMean, resultsSD, timeMean, timeSD);
        }

        private static void EvolutionaryIterativePopulationTest(int[][] graph)
        {
            int[] time = new int[25];
            for (int j = 0; j < 25; j++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                GeneticIterativeSearch gils = new GeneticIterativeSearch(100, graph.Length, new GraphBipartition(graph), new Crossover(), 1000);
                gils.Run();

                stopwatch.Stop();
                time[j] = (int)stopwatch.ElapsedMilliseconds;
            }

            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_EvolutionaryIterative.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Time";

            // Write the values into the table
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                float[] temp = new float[25];
                string line;
                for (int i = 0; i < time.Length; i++)
                {
                    line = string.Format("{0}", time[i]);
                    writer.WriteLine(line);
                    temp[i] = time[i];
                }
                line = string.Format("{0}", CalculateMean(temp));
                writer.WriteLine(line);
                line = string.Format("{0}", CalculateStandardDeviation(temp));
                writer.WriteLine(line);
                writer.Flush();
            }
        }

        // Save the results to a table
        private static void SaveTable(string path, string newHeader, int[] parameter, float[] resultsMean, float[] resultsSD, float[] timeMean, float[] timeSD)
        {
            // Create the the table and set the headers
            string header = "";
            if (!File.Exists(path))
                header = newHeader;

            // Write the values into the table
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                string line;
                for (int i = 0; i < parameter.Length; i++)
                {
                    line = string.Format("{0}, {1}, {2}, {3}, {4}", parameter[i], resultsMean[i], resultsSD[i], timeMean[i], timeSD[i]);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }

        // Calculate the mean of an array of values
        private static float CalculateMean(float[] values)
        {
            float mean = 0;
            foreach (float value in values)
                mean += value / values.Length;
            return mean;
        }

        // Calculate the standard deviation of an array of values and a mean
        private static float CalculateStandardDeviation(float[] values, float mean)
        {
            float[] intermediate = new float[values.Length];
            for (int i = 0; i < values.Length; ++i)
                intermediate[i] = (float)Math.Pow(values[i] - mean, 2);

            float standardDeviation = 0;
            foreach (float value in intermediate)
                standardDeviation += value / values.Length;
            return (float)Math.Sqrt(standardDeviation);
        }

        // Calculate the standard deviation of an array of values
        private static float CalculateStandardDeviation(float[] values)
        {
            return CalculateStandardDeviation(values, CalculateMean(values));
        }
    }
}
