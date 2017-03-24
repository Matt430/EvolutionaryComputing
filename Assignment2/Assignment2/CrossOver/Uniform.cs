﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Uniform : Crossover
    {
        public override List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random)
        {
            int length = population.Count;
            int stringLength = population[0].Count;

            for (int i = 0; i + 1 < length; i += 2)
            {
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
                    if (population[i][j] == population[i+ 1][j])
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
                population.Add(child1);
                population.Add(child2);
            }
            return (population);
        }
    }
}