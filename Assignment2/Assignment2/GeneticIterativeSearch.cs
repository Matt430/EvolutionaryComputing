using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Assignment2
{
    class GeneticIterativeSearch
    {
        public enum FitnessType { Uniform }

        private Random random = new Random();
        private Stopwatch stopwatch = new Stopwatch();

        private int populationCount;
        private FitnessFunction fitnessFunction;
        private Crossover crossover;
        private IteratedLocalSearch ils;
        private List<List<bool>> population;

        private int optimalFitness;
        private int firstHitGeneration, convergenceGeneration;
        private int localOptima;

        public int bestFitness1Min;

        public GeneticIterativeSearch(int populationCount, int stringLength, FitnessFunction fitnessFunction, Crossover crossover, int localOptima)
        {
            this.populationCount = populationCount;
            this.fitnessFunction = fitnessFunction;
            this.localOptima = localOptima;
            this.crossover = crossover;
            ils = new IteratedLocalSearch(stringLength, fitnessFunction, 15, localOptima / 10);

            //Generate a random population
            population = GenerateRandomPopulation(populationCount, stringLength);
        }

        public void Run()
        {
            bool oneMin = false;
            stopwatch.Start();

            //Main loop
            int generation = 1;
            int currentOptima = populationCount;
            LocalSearch localsearch = new LocalSearch(fitnessFunction as GraphBipartition);
            Parallel.For(0, populationCount, i =>
            {
                population[i] = localsearch.Search(population[i]);
            });

            while (PrintString(population[0]) != PrintString(population[populationCount - 1]))
            {
                //Make sure the ordering is random.
                population = ShufflePopulation(population);

                //Generate new offsring using the chosen crossover method.
                population = crossover.GenerateOffspring(population, random, localsearch, localOptima, currentOptima);
                currentOptima += populationCount;

                if (currentOptima >= localOptima - (localOptima / 10))
                {
                    population.Sort(fitnessFunction.FitnessCompare);
                    break;
                }

                if (stopwatch.ElapsedMilliseconds > 60000 && !oneMin)
                {
                    population.Sort(fitnessFunction.FitnessCompare);
                    bestFitness1Min = fitnessFunction.Fitness(population[0]);
                    oneMin = true;
                }

                //Sort the list and remove the worst half.
                population.Sort(fitnessFunction.FitnessCompare);
                population.RemoveRange(population.Count / 2, population.Count / 2);

                ++generation;
            }

            Console.WriteLine(fitnessFunction.Fitness(population[0]));
            population[0] = ils.Run(population[0]);

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine(PrintString(population[0]) + " " + fitnessFunction.Fitness(population[0]));
        }

        //This method generates a list of random bitstrings with a population size of size, where each string has a length of stringlength
        List<List<bool>> GenerateRandomPopulation(int size, int stringlength)
        {
            List<List<bool>> population = new List<List<bool>>();
            for (int i = 0; i < size; i++)
            {
                double neededZero = stringlength / 2, neededOne = stringlength / 2;
                List<bool> bitString = new List<bool>();
                for (int j = 0; j < stringlength; j++)
                {
                    if (random.NextDouble() >= (neededZero / (neededZero + neededOne)))
                    {
                        bitString.Add(true);
                        neededOne--;
                    }
                    else
                    {
                        bitString.Add(false);
                        neededZero--;
                    }
                }
                population.Add(bitString);
            }
            return population;
        }

        //Randomizes the order of the population list.
        List<List<bool>> ShufflePopulation(List<List<bool>> population)
        {
            int n = population.Count;
            while (n > 1)
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
            foreach (bool bit in bitstring)
            {
                if (bit)
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

        public int Result
        { get { return fitnessFunction.Fitness(population[0]); } }
    }
}
