using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Assignment2
{
    class GeneticAlgorithm
    {
        public enum FitnessType { Uniform }

        private Random random = new Random();
        private Stopwatch stopwatch = new Stopwatch();

        private int populationCount;
        private FitnessFunction fitnessFunction;
        private Crossover crossover;
        private List<List<bool>> population;

        private float optimalFitness;
        private int firstHitGeneration, convergenceGeneration;

        public GeneticAlgorithm(int populationCount, int stringLength, FitnessFunction fitnessFunction, Crossover crossover)
        {
            this.populationCount = populationCount;
            this.fitnessFunction = fitnessFunction;
            this.crossover = crossover;

            //Generate a random population
            population = GenerateRandomPopulation(populationCount, stringLength);
        }

        public void Run()
        {
            stopwatch.Start();

            //Main loop
            int generation = 1;
            while(PrintString(population[0]) != PrintString(population[populationCount - 1]))
            {
                //Make sure the ordering is random.
                population = ShufflePopulation(population);

                // Stop check
                List<List<bool>> populationCheck = new List<List<bool>>(population);
                populationCheck.Sort(fitnessFunction.FitnessCompare);

                //Generate new offsring using the choses crossover method.
                population = crossover.GenerateOffspring(population, random);

                //Sort the list and remove the worst half.
                population.Sort(fitnessFunction.FitnessCompare);
                population.RemoveRange(0, population.Count / 2);

                // Check if any children have entered the population
                if(populationCheck.SequenceEqual(population))
                {
                    Console.WriteLine("No new offspring was found!");
                    break;
                }

                ++generation;
            }
            convergenceGeneration = generation;
            //Console.WriteLine("Population convergence at: " + generation);

            stopwatch.Stop();
        }

        //This method generates a list of random bitstrings with a population size of size, where each string has a length of stringlength
        List<List<bool>> GenerateRandomPopulation(int size, int stringlength)
        {
            List<List<bool>> population = new List<List<bool>>();
            for(int i = 0; i < size; i++)
            {
                List<bool> bitString = new List<bool>();
                for(int j = 0; j < stringlength; j++)
                {
                    bitString.Add(random.NextDouble() >= 0.5);
                }
                population.Add(bitString);
            }
            return population;
        }

        //Randomizes the order of the population list.
        List<List<bool>> ShufflePopulation(List<List<bool>> population)
        {
            int n = population.Count;
            while(n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                List<bool> value = population[k];
                population[k] = population[n];
                population[n] = value;
            }
            return (population);
        }

        //This methods converts a list of bits to a printable string.
        string PrintString(List<bool> bitstring)
        {
            string bits = "";
            foreach(bool bit in bitstring)
            {
                if(bit)
                    bits += "1";
                else
                    bits += "0";
            }
            return bits;
        }

        public int FirstHitGeneration
        { get { return firstHitGeneration; } }

        public int ConvergenceGeneration
        { get { return convergenceGeneration; } }

        public int FitnessCalls
        { get { return fitnessFunction.FitnessCalls; } }

        public long RunTime
        { get { return stopwatch.ElapsedMilliseconds; } }
    }
}
