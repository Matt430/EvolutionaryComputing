using System;
using System.Threading.Tasks;

namespace Assignment1
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            //Assign how large the population should be.
            int[] populationCounts = new int[] { 50, 100, 250, 500 };
            //Assign string length.
            int stringLength = 100;
            //Assign length of subfunctions.
            int k = 4;
            //Assign deceptiveness. 1 = deceptive, 2.5 is non-deceptive.
            float d = 1f;

            //Assign which fitness function and crossover must be used.
            //FitnessFunction fitnessFunction = new LooseLinkTrap(k, d, 100, random);
            //Crossover crossover = new UniformCrossover();
            FitnessFunction fitnessFunction = new UniformOneCount();
            Crossover crossover = new TwoPointCrossover();

            for(int r = 0; r < 4; ++r)
            {
                int numberOfRuns = 25;
                float[] firstHits = new float[numberOfRuns];
                float[] convergences = new float[numberOfRuns];
                float[] runTimes = new float[numberOfRuns];
                Parallel.For(0, numberOfRuns, i =>
                {
                    GeneticAlgorithm ga = new GeneticAlgorithm(populationCounts[r], stringLength, fitnessFunction, crossover);
                    ga.Run();

                    firstHits[i] = ga.FirstHitGeneration;
                    convergences[i] = ga.ConvergenceGeneration;
                    runTimes[i] = ga.RunTime;
                });

                // Calculate successes
                int successes = 0;
                foreach(float firstHit in firstHits)
                    if(firstHit > 0)
                        ++successes;

                // Calculate the mean
                float meanFirstHit = CalculateMean(firstHits);
                float meanConvergence = CalculateMean(convergences);
                float meanRuntime = CalculateMean(runTimes);

                // Calculate the standard deviation
                float firstHitSD = CalculateStandardDeviation(firstHits, meanFirstHit);
                float convergenceSD = CalculateStandardDeviation(convergences, meanConvergence);
                float runTimeSD = CalculateStandardDeviation(runTimes, meanRuntime);

                Console.WriteLine("Population Size: " + populationCounts[r % populationCounts.Length]);
                Console.WriteLine("Successes: " + successes);
                Console.WriteLine("First Hit: " + meanFirstHit + " (" + firstHitSD + ")");
                Console.WriteLine("Convergence: " + meanConvergence + " (" + convergenceSD + ")");
                Console.WriteLine("Run Time: " + meanRuntime + " (" + runTimeSD + ")");
            }

            Console.WriteLine("Simulation over press any key to close program...");
            Console.ReadKey();
        }

        // Calculate the mean of an array of values
        static float CalculateMean(float[] values)
        {
            float mean = 0;
            foreach(float value in values)
                mean += value / values.Length;
            return mean;
        }

        // Calculate the standard deviation of an array of values and a mean
        static float CalculateStandardDeviation(float[] values, float mean)
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
        static float CalculateStandardDeviation(float[] values)
        {
            return CalculateStandardDeviation(values, CalculateMean(values));
        }
    }
}
