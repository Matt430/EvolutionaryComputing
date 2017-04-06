using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Assignment2
{
    class Crossover
    {
        public List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random, LocalSearch localsearch, int localOptima, int currentOptima)
        {
            int length = population.Count;
            int stringLength = population[0].Count;

            for (int i = 0; i + 1 < length; i += 2)
            {
                if (currentOptima >= localOptima)
                    break;

                //Calculate Hamming Distance
                int hamming = 0;
                for (int j = 0; j < stringLength; j++)
                {
                    if (population[i][j] != population[i + 1][j])
                        hamming++;
                }
                //Flip if Hamming Distance is greater than l / 2.
                if (hamming > (stringLength / 2))
                {
                    for (int j = 0; j < stringLength; j++)
                    {
                        if (population[i][j])
                            population[i][j] = false;
                        else
                            population[i][j] = true;
                    }
                    hamming = stringLength - hamming;
                }

                double neededZero = hamming / 2, neededOne = hamming / 2;
                // Create the children
                List<bool> child1 = new List<bool>(), child2 = new List<bool>();
                for (int j = 0; j < stringLength; j++)
                {
                    if (population[i][j] == population[i + 1][j])
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i][j]);
                    }
                    else if (random.NextDouble() >= (neededZero / (neededZero + neededOne)))
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i + 1][j]);
                        neededOne--;
                    }
                    else
                    {
                        child1.Add(population[i + 1][j]);
                        child2.Add(population[i][j]);
                        neededZero--;
                    }
                }
                Thread thread1 = new Thread(() => child1 = localsearch.Search(child1));
                Thread thread2 = new Thread(() => child2 = localsearch.Search(child2));
                thread1.Start();
                thread2.Start();
                thread1.Join();
                thread2.Join();
                currentOptima += 2;
                population.Add(child1);
                population.Add(child2);

                if (currentOptima >= localOptima)
                    return population;
            }
            return population;
        }

        public List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random, LocalSearch localsearch, FitnessFunction fitnessFunction, int localOptima, int currentOptima)
        {
            List<List<bool>> newPopulation = new List<List<bool>>();
            int length = population.Count;
            int stringLength = population[0].Count;

            for (int i = 0; i + 1 < length; i += 2)
            {
                if (currentOptima >= localOptima)
                    break;

                //Calculate Hamming Distance
                int hamming = 0;
                for (int j = 0; j < stringLength; j++)
                {
                    if (population[i][j] != population[i + 1][j])
                        hamming++;
                }
                //Flip if Hamming Distance is greater than l / 2.
                if (hamming > (stringLength / 2))
                {
                    for (int j = 0; j < stringLength; j++)
                    {
                        if (population[i][j])
                            population[i][j] = false;
                        else
                            population[i][j] = true;
                    }
                    hamming = stringLength - hamming;
                }

                double neededZero = hamming / 2, neededOne = hamming / 2;
                // Create the children
                List<bool> child1 = new List<bool>(), child2 = new List<bool>();
                for (int j = 0; j < stringLength; j++)
                {
                    if (population[i][j] == population[i + 1][j])
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i][j]);
                    }
                    else if (random.NextDouble() >= (neededZero / (neededZero + neededOne)))
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i + 1][j]);
                        neededOne--;
                    }
                    else
                    {
                        child1.Add(population[i + 1][j]);
                        child2.Add(population[i][j]);
                        neededZero--;
                    }
                }
                Thread thread1 = new Thread(() => child1 = localsearch.Search(child1));
                Thread thread2 = new Thread(() => child2 = localsearch.Search(child2));
                thread1.Start();
                thread2.Start();
                thread1.Join();
                thread2.Join();
                currentOptima += 2;

                List<List<bool>> family = new List<List<bool>>();
                family.Add(population[i]);
                family.Add(population[i + 1]);
                family.Add(child1);
                family.Add(child2);
                family.Sort(fitnessFunction.FitnessCompare);

                newPopulation.AddRange(family.Take(2));

                if (currentOptima >= localOptima)
                    return newPopulation;
            }
            return newPopulation;
        }
    }
}
