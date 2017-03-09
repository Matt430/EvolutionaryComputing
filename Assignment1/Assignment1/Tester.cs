using System;
using System.IO;
using System.Threading.Tasks;

namespace Assignment1
{
    class Tester
    {
        // Run 25 tests with the given settings
        public static void RunTests(int[] populationCounts, int stringLength, int k, float d, GeneticAlgorithm.FitnessType fitnessType)
        {
            for(int c = 0; c < 2; ++c)
            {
                string crossoverType = "";
                if(c == 0)
                    crossoverType = "2X";
                else
                    crossoverType = "UX";

                Console.WriteLine("Start Test " + crossoverType + " " + fitnessType.ToString() + " with d:" + d + "...");
                for(int r = 0; r < 4; ++r)
                {
                    int numberOfRuns = 25;

                    // Containers for the results
                    float[] firstHits = new float[numberOfRuns];
                    float[] convergences = new float[numberOfRuns];
                    float[] fitnessCalls = new float[numberOfRuns];
                    float[] runTimes = new float[numberOfRuns];

                    Parallel.For(0, numberOfRuns, i =>
                    //for(int i = 0; i < numberOfRuns; ++i)
                    {
                        // Assign which crossover must be used
                        Crossover crossover;
                        if(c == 0)
                            crossover = new TwoPointCrossover();
                        else
                            crossover = new UniformCrossover();

                        // Assign which fitness function must be used
                        FitnessFunction fitnessFunction;
                        switch(fitnessType)
                        {
                            case GeneticAlgorithm.FitnessType.Uniform:
                                fitnessFunction = new UniformOneCount();
                                break;
                            case GeneticAlgorithm.FitnessType.Linear:
                                fitnessFunction = new LinearOneCount();
                                break;
                            case GeneticAlgorithm.FitnessType.TightLink:
                                fitnessFunction = new TightLinkTrap(k, d);
                                break;
                            default:
                                fitnessFunction = new LooseLinkTrap(k, d, stringLength);
                                break;
                        }

                        // Run the test
                        GeneticAlgorithm ga = new GeneticAlgorithm(populationCounts[r], stringLength, fitnessFunction, crossover);
                        ga.Run();

                        firstHits[i] = ga.FirstHitGeneration;
                        convergences[i] = ga.ConvergenceGeneration;
                        fitnessCalls[i] = ga.FitnessCalls;
                        runTimes[i] = ga.RunTime;
                    });

                    // Calculate successes
                    int successes = 0;
                    foreach(float firstHit in firstHits)
                        if(firstHit > 0)
                            ++successes;
                    float[] newFirstHits = new float[successes];
                    int n = 0;
                    foreach (float firstHit in firstHits)
                        if (firstHit > 0)
                        {
                            newFirstHits[n] = firstHit;
                            ++n;
                        }

                    // Calculate the mean
                    float meanFirstHit = CalculateMean(newFirstHits);
                    float meanConvergence = CalculateMean(convergences);
                    float meanFitnessCalls = CalculateMean(fitnessCalls);
                    float meanRuntime = CalculateMean(runTimes);

                    // Calculate the standard deviation
                    float firstHitSD = CalculateStandardDeviation(newFirstHits, meanFirstHit);
                    float convergenceSD = CalculateStandardDeviation(convergences, meanConvergence);
                    float fitnessCallsSD = CalculateStandardDeviation(fitnessCalls, meanFitnessCalls);
                    float runTimeSD = CalculateStandardDeviation(runTimes, meanRuntime);

                    // Save the results in a .csv file
                    SaveTable(crossoverType, fitnessType.ToString(), d, populationCounts[r], successes, meanFirstHit, firstHitSD, meanConvergence, convergenceSD, meanFitnessCalls, fitnessCallsSD, meanRuntime, runTimeSD);
                }
                Console.WriteLine("Finished Test!");
            }
        }

        // Save the results to a table
        private static void SaveTable(string crossover, string fitnessType, float d,
            int populationSize, int successes,
            float meanFirstHit, float firstHitSD,
            float meanConvergence, float convergenceSD,
            float meanFitnessCalls, float fitnessCallsSD,
            float meanRuntime, float runTimeSD)
        {
            // Create the the table and set the headers
            string path = Directory.GetCurrentDirectory() + "/Results/ResultTable_" + crossover + "_" + fitnessType + "_" + d + ".csv";
            string header = "";
            if(!File.Exists(path))
                header = "Population Size|Successes|Mean First Hit|Standard Deviation First Hit|Mean Convergence|Standard Deviation Convergence|Mean Function Calls|Standard Deviation Function Calls|Mean Runtime|Standard Deviation Runtime";

            // Write the values into the table
            using(StreamWriter writer = new StreamWriter(path, true))
            {
                if(header != "")
                    writer.WriteLine(header);
                string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", populationSize, successes, meanFirstHit, firstHitSD, meanConvergence, convergenceSD, meanFitnessCalls, fitnessCallsSD, meanRuntime, runTimeSD);
                writer.WriteLine(line);
                writer.Flush();
            }
        }

        // Calculate the mean of an array of values
        private static float CalculateMean(float[] values)
        {
            float mean = 0;
            foreach(float value in values)
                mean += value / values.Length;
            return mean;
        }

        // Calculate the standard deviation of an array of values and a mean
        private static float CalculateStandardDeviation(float[] values, float mean)
        {
            float[] intermediate = new float[values.Length];
            for(int i = 0; i < values.Length; ++i)
                intermediate[i] = (float) Math.Pow(values[i] - mean, 2);

            float standardDeviation = 0;
            foreach(float value in intermediate)
                standardDeviation += value / values.Length;
            return (float) Math.Sqrt(standardDeviation);
        }

        // Calculate the standard deviation of an array of values
        private static float CalculateStandardDeviation(float[] values)
        {
            return CalculateStandardDeviation(values, CalculateMean(values));
        }
    }
}
