using System;
using System.IO;

namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Assign how large the population should be.
            int[] populationCounts = new int[] { 50, 100, 250, 500 };
            //Assign string length.
            int stringLength = 100;
            //Assign length of subfunctions.
            int k = 4;

            Console.WriteLine("Start Testing...");

            // Clear results
            string path = Directory.GetCurrentDirectory() + "/Results/";
            if(Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            // Run the test
            Tester.RunTests(populationCounts, stringLength, k, 0f, GeneticAlgorithm.FitnessType.Uniform);
            Tester.RunTests(populationCounts, stringLength, k, 0f, GeneticAlgorithm.FitnessType.Linear);
            Tester.RunTests(populationCounts, stringLength, k, 1f, GeneticAlgorithm.FitnessType.TightLink);
            Tester.RunTests(populationCounts, stringLength, k, 2.5f, GeneticAlgorithm.FitnessType.TightLink);
            Tester.RunTests(populationCounts, stringLength, k, 1f, GeneticAlgorithm.FitnessType.LooseLink);
            Tester.RunTests(populationCounts, stringLength, k, 2.5f, GeneticAlgorithm.FitnessType.LooseLink);

            Console.WriteLine("Testing Finished!");
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
        }
    }
}
