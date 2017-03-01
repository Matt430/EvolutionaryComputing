using System;
using System.Collections.Generic;

namespace Assignment1
{
    class UniformCrossover : Crossover
    {
        public override List<List<bool>> GenerateOffspring(List<List<bool>> population, Random random)
        {
            int length = population.Count;
            int stringLength = population[0].Count;

            for(int i = 0; i + 1 < length; i += 2)
            {
                // Create the children
                List<bool> child1 = new List<bool>(), child2 = new List<bool>();
                for(int j = 0; j < stringLength; j++)
                {
                    if(random.NextDouble() >= 0.5)
                    {
                        child1.Add(population[i][j]);
                        child2.Add(population[i + 1][j]);
                    }
                    else
                    {
                        child1.Add(population[i + 1][j]);
                        child2.Add(population[i][j]);
                    }
                }
                population.Add(child1);
                population.Add(child2);
            }
            return (population);
        }
    }
}
