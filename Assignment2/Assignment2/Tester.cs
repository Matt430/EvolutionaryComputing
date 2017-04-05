using System;
using System.Diagnostics;
using System.IO;

namespace Assignment2
{
    class Tester
    {
        // Run 25 tests with the given settings
        public static void RunTests(int[][] graph, int noRuns, int localOptima)
        {
            Console.WriteLine("Start Testing MLS...");
            MLSTest(graph, noRuns, localOptima);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing ILS...");
            ILSMutationTest(graph, noRuns, localOptima);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing GLS...");
            EvolutionaryPopulationTest(graph, noRuns, localOptima);
            Console.WriteLine("Testing Finished!");

            Console.WriteLine("Start Testing GILS...");
            EvolutionaryIterativePopulationTest(graph, noRuns, localOptima);
            Console.WriteLine("Testing Finished!");
        }

        private static void MLSTest(int[][] graph, int noRuns, int localOptima)
        {
            int[] results = new int[noRuns];
            int[] time = new int[noRuns];
            for (int i = 0; i < noRuns; ++i)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                MultiStartLocalSearch mls = new MultiStartLocalSearch(graph.Length, new GraphBipartition(graph), localOptima);
                mls.Run();

                stopwatch.Stop();
                results[i] = mls.bestFitness;
                time[i] = (int)stopwatch.ElapsedMilliseconds;
            }

            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_MLS.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Results, Time";

            // Write the values into the table
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                float[] temp = new float[noRuns];
                float[] temp2 = new float[noRuns];
                string line;
                for (int i = 0; i < time.Length; i++)
                {
                    line = string.Format("{0}, {1}", results[i], time[i]);
                    writer.WriteLine(line);
                    temp[i] = results[i];
                    temp2[i] = time[i];
                }
                line = string.Format("{0}, {1}", CalculateMean(temp).ToString("0.0000"), CalculateMean(temp2).ToString("0.0000"));
                writer.WriteLine(line);
                line = string.Format("{0}, {1}", CalculateStandardDeviation(temp).ToString("0.0000"), CalculateStandardDeviation(temp2).ToString("0.0000"));
                writer.WriteLine(line);
                writer.Flush();
            }
        }

        private static void ILSMutationTest(int[][] graph, int noRuns, int localOptima)
        {
            int[] parameter = new int[15];
            float[] resultsMean = new float[15];
            float[] resultsSD = new float[15];
            float[] timeMean = new float[15];
            float[] timeSD = new float[15];
            int n = 0;

            for (int i = 5; i <= 25; i += 5)
            {
                float[] resultValues = new float[noRuns];
                float[] timeValues = new float[noRuns];
                for (int j = 0; j < noRuns; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    IteratedLocalSearch ils = new IteratedLocalSearch(graph.Length, new GraphBipartition(graph), i, localOptima);
                    ils.Run();

                    stopwatch.Stop();

                    parameter[n] = i;
                    resultValues[j] = ils.bestValue;
                    timeValues[j] = (int)stopwatch.ElapsedMilliseconds;
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

        private static void EvolutionaryPopulationTest(int[][] graph, int noRuns, int localOptima)
        {
            int[] parameter = new int[5];
            float[] resultsMean = new float[5];
            float[] resultsSD = new float[5];
            float[] timeMean = new float[5];
            float[] timeSD = new float[5];
            int n = 0;

            for (int i = 50; i <= 250; i += 50)
            {
                float[] resultValues = new float[noRuns];
                float[] timeValues = new float[noRuns];
                for (int j = 0; j < noRuns; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    GeneticAlgorithm ga = new GeneticAlgorithm(i, graph.Length, new GraphBipartition(graph), new Crossover(), localOptima);
                    ga.Run();

                    stopwatch.Stop();

                    parameter[n] = i;
                    resultValues[j] = ga.Result;
                    timeValues[j] = (int)stopwatch.ElapsedMilliseconds;
                }
                resultsMean[n] = CalculateMean(resultValues);
                resultsSD[n] = CalculateStandardDeviation(resultValues);
                timeMean[n] = CalculateMean(timeValues);
                timeSD[n] = CalculateStandardDeviation(timeValues);
                n++;
            }

            // Save results
            SaveTable(Directory.GetCurrentDirectory() + "/Results/ResultTable_GLS_Parameters.csv", "Parameter, Results Mean, Results SD, Time Mean, Time SD", parameter, resultsMean, resultsSD, timeMean, timeSD);
        }

        private static void EvolutionaryIterativePopulationTest(int[][] graph, int noRuns, int localOptima)
        {
            int[] results = new int[noRuns];
            int[] time = new int[noRuns];
            for (int j = 0; j < noRuns; j++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                GeneticIterativeSearch gils = new GeneticIterativeSearch(100, graph.Length, new GraphBipartition(graph), new Crossover(), localOptima);
                gils.Run();

                stopwatch.Stop();
                results[j] = gils.Result;
                time[j] = (int)stopwatch.ElapsedMilliseconds;
            }

            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_EvolutionaryIterative.csv";
            string header = "";
            if (!File.Exists(path))
                header = "Results, Time";

            // Write the values into the table
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (header != "")
                    writer.WriteLine(header);
                float[] temp = new float[noRuns];
                float[] temp2 = new float[noRuns];
                string line;
                for (int i = 0; i < time.Length; i++)
                {
                    line = string.Format("{0}, {1}", results[i], time[i]);
                    writer.WriteLine(line);
                    temp[i] = results[i];
                    temp2[i] = time[i];
                }
                line = string.Format("{0}, {1}", CalculateMean(temp).ToString("0.0000"), CalculateMean(temp2).ToString("0.0000"));
                writer.WriteLine(line);
                line = string.Format("{0}, {1}", CalculateStandardDeviation(temp).ToString("0.0000"), CalculateStandardDeviation(temp2).ToString("0.0000"));
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
                    line = string.Format("{0}, {1}, {2}, {3}, {4}", parameter[i], resultsMean[i].ToString("0.0000"), resultsSD[i].ToString("0.0000"), timeMean[i].ToString("0.0000"), timeSD[i].ToString("0.0000"));
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
