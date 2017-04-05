using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Assignment2
{
    class MultiStartLocalSearch
    {
        int startCount;
        GraphBipartition fitnessFunction;
        private Random random = new Random();
        private List<List<bool>> startValues;
        LocalSearch localSearch;

        public MultiStartLocalSearch(int stringLength, FitnessFunction fitnessFunction, int localOptima)
        {
            startCount = localOptima;
            this.fitnessFunction = fitnessFunction as GraphBipartition;
            localSearch = new LocalSearch(this.fitnessFunction);

            //Generate a random population
            startValues = GenerateRandomPopulation(localOptima, stringLength);
        }

        public void Run()
        {
            List<bool> bestResult = startValues[0];
            int bestFitness = int.MaxValue;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.ForEach(startValues, value =>
            //foreach (List<bool> value in startValues)
            {
                List<bool> currentResult = localSearch.Search(value);
                int currentFitness = fitnessFunction.Fitness(currentResult);

                if (currentFitness < bestFitness)
                {
                    bestResult = currentResult;
                    bestFitness = currentFitness;
                }
            });

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds);

            Console.WriteLine(PrintString(bestResult) + " " + bestFitness);
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
    }
}
