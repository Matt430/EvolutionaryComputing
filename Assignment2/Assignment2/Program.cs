using System;
using System.IO;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Assign how large the population should be.
            //int[] populationCounts = new int[] { 50, 100, 250, 500 };

            Console.WriteLine("Start Testing...");

            // Clear results
            string path = Directory.GetCurrentDirectory() + "/Results/";
            if(Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            // Run the test
            //Tester.RunTests(populationCounts, stringLength, k, 0f, GeneticAlgorithm.FitnessType.Uniform);

            Console.WriteLine("Testing Finished!");
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
        }
    }
}
