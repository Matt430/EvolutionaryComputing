using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assignment2
{
    class IteratedLocalSearch
    {
        GraphBipartition fitnessFunction;
        private Random random = new Random();
        LocalSearch localSearch;
        int mutateSwaps;
        int stringLength;
        int localOptima;
        public int bestValue;
        public int bestFitness1Min;

        public IteratedLocalSearch(int stringLength, FitnessFunction fitnessFunction, int mutateSwaps, int localOptima)
        {
            this.fitnessFunction = fitnessFunction as GraphBipartition;
            localSearch = new LocalSearch(this.fitnessFunction);
            this.mutateSwaps = mutateSwaps;
            this.stringLength = stringLength;
            this.localOptima = localOptima;
        }

        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<bool> bestSolution = Run(GenerateRandomBitstring(stringLength));

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds);

            bestValue = fitnessFunction.Fitness(bestSolution);
            Console.WriteLine(PrintString(bestSolution) + " " + fitnessFunction.Fitness(bestSolution));
        }

        public List<bool> Run(List<bool> bitString)
        {
            bool oneMin = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<bool> currentSolution = localSearch.Search(bitString);
            for (int i = 1; i < localOptima; i++)
            {
                List<bool> newSolution = localSearch.Search(MutateString(currentSolution));
                if (fitnessFunction.Fitness(newSolution) < fitnessFunction.Fitness(currentSolution))
                {
                    currentSolution = newSolution;
                }

                if (stopwatch.ElapsedMilliseconds > 60000 && !oneMin)
                {
                    bestFitness1Min = fitnessFunction.Fitness(currentSolution);
                    oneMin = true;
                }
            }

            stopwatch.Stop();
            return currentSolution;
        }

        private List<bool> MutateString(List<bool> bitString)
        {
            for (int i = 0; i < mutateSwaps; i++)
            {
                int swap1 = random.Next(bitString.Count);
                int swap2 = random.Next(bitString.Count);
                while (bitString[swap1] == bitString[swap2])
                {
                    swap1 = random.Next(bitString.Count);
                    swap2 = random.Next(bitString.Count);
                }

                bitString[swap1] = !bitString[swap1];
                bitString[swap2] = !bitString[swap2];
            }
            return bitString;
        }

        private List<bool> GenerateRandomBitstring(int stringlength)
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
            return bitString;
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
